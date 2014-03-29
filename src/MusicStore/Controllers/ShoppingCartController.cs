﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using Microsoft.AspNet.Mvc;
using MusicStore.Models;
using MusicStore.ViewModels;
using System.Linq;

namespace MusicStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        private MusicStoreContext db = new MusicStoreContext();

        //
        // GET: /ShoppingCart/

        public IActionResult Index()
        {
            var cart = ShoppingCart.GetCart(db, this.Context);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            // Return the view
            return View(viewModel);
        }

        //
        // GET: /ShoppingCart/AddToCart/5

        public IActionResult AddToCart(int id)
        {

            // Retrieve the album from the database
            var addedAlbum = db.Albums
                .Single(album => album.AlbumId == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(db, this.Context);

            cart.AddToCart(addedAlbum);

            db.SaveChanges();

            // Go back to the main store page for more shopping
            //Bug: Helper method not available
            //return RedirectToAction("Index");
            return View();
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5

        //Bug: Missing HTTP verb attribute
        //[HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            // Retrieve the current user's shopping cart
            var cart = ShoppingCart.GetCart(db, this.Context);

            // Get the name of the album to display confirmation
            // TODO [EF] Turn into one query once query of related data is enabled
            int albumId = db.Carts.Single(item => item.CartItemId == id).AlbumId;
            string albumName = db.Albums.Single(a => a.AlbumId == albumId).Title;

            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            db.SaveChanges();

            string removed = (itemCount > 0) ? " 1 copy of " : string.Empty;

            // Display the confirmation message

            var results = new ShoppingCartRemoveViewModel
            {
                Message = removed + albumName +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };

            //Bug: Missing helper
            //return Json(results);
            return new JsonResult(results);
        }
    }
}