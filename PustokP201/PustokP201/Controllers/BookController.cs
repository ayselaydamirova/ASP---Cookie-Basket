using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PustokP201.Models;
using PustokP201.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PustokP201.Controllers
{
    public class BookController : Controller
    {
        private readonly PustokContext _context;
        public BookController(PustokContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult SetSession(int id)
        {
            HttpContext.Session.SetString("bookId",id.ToString());

            return Content("added");
        }

        public ActionResult GetSession()
        {
            string idStr = HttpContext.Session.GetString("bookId");

            return Content(idStr);
        }

        public IActionResult SetCookie(int id)
        {
            HttpContext.Response.Cookies.Append("bookId", id.ToString());
            return Content("cookie");
        }

        public IActionResult GetCookie(string key)
        {
            string str = HttpContext.Request.Cookies[key];

            return Content(str);
        }

        public IActionResult AddBasket(int bookId)
        {
            if (!_context.Books.Any(x => x.Id == bookId))
            {
                return NotFound();
            }


            List<BasketItemViewModel> basketItems = new List<BasketItemViewModel>();
            string existBasketItems = HttpContext.Request.Cookies["basketItemList"];

            if (existBasketItems != null)
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(existBasketItems);
            }

            BasketItemViewModel item = basketItems.FirstOrDefault(x => x.BookId == bookId);

            if (item == null)
            {
                item = new BasketItemViewModel
                {
                    BookId = bookId,
                    Count = 1
                };
                basketItems.Add(item);
            }
            else
            {
                item.Count++;
            }


            var bookIdsStr = JsonConvert.SerializeObject(basketItems);

            HttpContext.Response.Cookies.Append("basketItemList", bookIdsStr);

            return Ok();
        }

        public IActionResult ShowBasket()
        {
            var bookIdsStr = HttpContext.Request.Cookies["basketItemList"];
            List<BasketItemViewModel> bookIds = new List<BasketItemViewModel>();
            if (bookIdsStr != null)
            {
                bookIds = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(bookIdsStr);
            }

            return Json(bookIds);
        }

        public IActionResult Checkout()
        {
            List<CheckoutItemViewModel> checkoutItems = new List<CheckoutItemViewModel>();

            string basketItemsStr = HttpContext.Request.Cookies["basketItemList"];
            if (basketItemsStr != null)
            {
                List<BasketItemViewModel> basketItems = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketItemsStr);

                foreach (var item in basketItems)
                {
                    CheckoutItemViewModel checkoutItem = new CheckoutItemViewModel
                    {
                        Book = _context.Books.FirstOrDefault(x => x.Id == item.BookId),
                        Count = item.Count
                    };
                    checkoutItems.Add(checkoutItem);
                }
            }

            return View(checkoutItems);
        }

    }
}
