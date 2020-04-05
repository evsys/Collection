using Collection.Models;
using Collection.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Collection.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext db;
        UserManager<User> _userManager;

        public HomeController(ApplicationContext context, UserManager<User> userManager)
        {
            db = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            List<Item> items = db.Items.OrderByDescending(x => x.Id).Take(3).ToList();

            for (int i = 0; i < items.Count; i++)
            {
                items[i].Collection = await db.CollectionDbs.FirstOrDefaultAsync(x => x.Id == items[i].IdCollection);
            }

            List<CollectionDb> collectionDbs = db.CollectionDbs.ToList();

            for (int i = 0; i < collectionDbs.Count; i++)
            {
                collectionDbs[i].CountItems = db.Items.Where(item => item.IdCollection == collectionDbs[i].Id).Count();
            }

            ViewBag.Collections = collectionDbs.OrderByDescending(x => x.CountItems).Take(3);

            return View(items);
        }

        public IActionResult AllUsers() => View(_userManager.Users.ToList());

        public async Task<IActionResult> UserPage(string userName)
        {
            User user = await _userManager.FindByEmailAsync(userName);
            List<CollectionDb> collectionDb = db.CollectionDbs.Where(x => x.IdUser == user.Id).ToList();
            ViewBag.OwnerName = user.Email;
            return View(collectionDb);
        }

        public async Task<IActionResult> ItemPage(int id)
        {
            List<Item> items = db.Items.Where(x => x.IdCollection == id).ToList();

            CollectionDb collectionDb = await db.CollectionDbs.FirstOrDefaultAsync(x => x.Id == id);

            if (items.Count == 0)
            {
                Item item = new Item
                {
                    IdCollection = collectionDb.Id,
                    Collection = collectionDb,
                    Id = 0
                };

                items.Add(item);
            }
            else
            {
                items[0].Collection = collectionDb;
            }

            ViewBag.OwnerName = await db.Users.FirstOrDefaultAsync(x => x.Id == collectionDb.IdUser);
            return View(items);
        }

        public async Task<IActionResult> Item(int id) 
        {
            Item item = await db.Items.FirstOrDefaultAsync(x => x.Id == id);
            CollectionDb collection = await db.CollectionDbs.FirstOrDefaultAsync(x => x.Id == item.IdCollection);

            item.Collection = collection;
            return View(item);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> CreateCollection(CollectionViewModel collectionViewModel)
        {
            List<Field> fields = new List<Field>();

            for (int i = 0; i < 3; i++)
            {
                fields.Add(new Field(collectionViewModel.NameField[i], collectionViewModel.TypeField[i]));
            }

            User ownerCollection = await db.Users.FirstOrDefaultAsync(u => u.Email == collectionViewModel.NameUser);

            CollectionDb collectionDb = new CollectionDb(collectionViewModel.NameCollection, collectionViewModel.Description, collectionViewModel.Theme)
            {
                User = ownerCollection,
                IdUser = ownerCollection.Id,
                FormattedFields = fields
            };

            db.CollectionDbs.Add(collectionDb);
            await db.SaveChangesAsync();
            return RedirectToAction("UserPage", "Home", new { userName = ownerCollection.Email });
        }

        public async Task<IActionResult> DeleteCollection(int idCollection)
        {
            CollectionDb collectionDb = await db.CollectionDbs.FirstOrDefaultAsync(x => x.Id == idCollection);
            User user = await db.Users.FirstOrDefaultAsync(u => u.Id == collectionDb.IdUser);
            db.Remove(collectionDb);
            await db.SaveChangesAsync();
            return RedirectToAction("UserPage", "Home", new { UserName = user.Email });
        }

        public async Task<IActionResult> CreateItem(ItemModel itemModel)
        {
            Item item = new Item
            {
                IdCollection = itemModel.IdCollection,
                Name = itemModel.ItemName,
                FormattedValues = itemModel.ItemValue,
                Tegs = itemModel.Tegs
            };

            db.Items.Add(item);
            await db.SaveChangesAsync();
            return RedirectToAction("ItemPage", "Home", new { Id = item.IdCollection });
        }

        public async Task<IActionResult> DeleteItem(int idItem)
        {
            Item item = await db.Items.FirstOrDefaultAsync(x => x.Id == idItem);

            int collectionId = item.IdCollection;
            db.Items.Remove(item);
            await db.SaveChangesAsync();
            return RedirectToAction("ItemPage", "Home", new { Id = collectionId });
        }
    }
}