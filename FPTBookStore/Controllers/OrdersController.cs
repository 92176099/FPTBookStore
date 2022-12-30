using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPTBookStore.Models;

namespace FPTBookStore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly FPTBookContext _context;

        public OrdersController(FPTBookContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            return View(await _context.Order.ToListAsync());
        }
        public async Task<IActionResult> Cart()
        {
            dynamic model = new System.Dynamic.ExpandoObject();
            var orders = getCartId(User.Identity.Name);
            var orders2 = await (from o in _context.OrderItem
                                 join i in _context.Books on o.BookId equals i.BookId
                                 select new
                                 {
                                     BookId = o.BookId,
                                     BookName = i.BookName,
                                     Quantity = o.Quantity
            ,
                                     Price = i.BookPrice
                                 }).ToListAsync();
            List<OrderItem> orderItems = await _context.OrderItem.Where(s => s.OrderId == orders).ToListAsync();
            model.OrderItems = orderItems;
            model.OrderItems2= orders2;
            model.OrderId = orders;
            return View("cart", model);
        }
        public int getCartId(string userName)
        {
            var order = _context.Order.FirstOrDefault(s => s.UserName == userName && s.OrderStatus == 0);
            if (order == null)
                order = CreateEmpty(User.Identity.Name);
            return order.OrderId;
        }
        // GET: Orders/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }
        public Order CreateEmpty(string userName)
        {
            Order order = new Order();
            order.UserName = userName;
            order.OrderStatus = 0;
            _context.Order.Add(order);
            _context.SaveChangesAsync();
            return order;
        }
        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,UserName,OrderAddress,OrderDate,OrderTotal,OrderPhone,OrderName,OrderStatus")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder( int orderId, string address, string name, string phone, int total)
        {
           var order= _context.Order.Find(orderId);
            order.OrderStatus = 1;
            order.OrderTotal = total;
            order.OrderPhone = phone;
            order.OrderAddress = address;
            order.OrderName = name;
            _context.Update(order);
            await _context.SaveChangesAsync();
            CreateEmpty(User.Identity.Name);
            return View(order);
        }
        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,UserName,OrderAddress,OrderDate,OrderTotal,OrderPhone,OrderName,OrderStatus")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Order == null)
            {
                return Problem("Entity set 'FPTBookContext.Order'  is null.");
            }
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }
    }
}
