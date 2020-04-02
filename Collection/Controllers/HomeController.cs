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
        private readonly ILogger<HomeController> _logger;
        UserManager<User> _userManager;

        public HomeController(ApplicationContext context, ILogger<HomeController> logger, UserManager<User> userManager)
        {
            db = context;
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index() => View(_userManager.Users.ToList());

        public async Task<IActionResult> UserPage(string userId)
        {
            User ownerCollections = await _userManager.FindByIdAsync(userId);
            List<CollectionDb> collectionDb = db.CollectionDbs.Where(x => x.IdUser == ownerCollections.Id).ToList();
            ViewBag.Name = userId;
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

            ViewBag.OwnerCollection = await db.Users.FirstOrDefaultAsync(x => x.Id == collectionDb.IdUser);
            return View(items);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(List<string> checkedId)
        {
            IQueryable<User> removedUsers = db.Users.Where(user => checkedId.Contains(user.Id));

            List<string> removedEmails = new List<string>();

            foreach (var user in removedUsers)
            {
                db.Users.Remove(user);
            }
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Lock(List<string> checkedId)
        {
            IQueryable<User> users = db.Users.Where(user => checkedId.Contains(user.Id));
            foreach (var user in users)
            {
                user.Status = "Заблокирован";
            }
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Unlock(List<string> checkedId)
        {
            IQueryable<User> users = db.Users.Where(user => checkedId.Contains(user.Id));
            foreach (var user in users)
            {
                user.Status = "Разблокирован";
            }
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CreateCollection(CollectionModel collectionModel, string userId)
        {
            List<Field> fields = new List<Field>();

            if (collectionModel.NameField != null)
            {
                for (int i = 0; i < collectionModel.NameField.Count; i++)
                {
                    fields.Add(new Field(collectionModel.NameField[i], collectionModel.TypeField[i]));
                }
            }

            User ownerCollection = await _userManager.FindByIdAsync(userId);

            CollectionDb collectionDb = new CollectionDb(collectionModel.NameCollection, collectionModel.Description)
            {
                User = ownerCollection,
                IdUser = ownerCollection.Id,
                FormattedFields = fields
            };

            db.CollectionDbs.Add(collectionDb);
            await db.SaveChangesAsync();
            return RedirectToAction("UserPage", "Home", new { UserId = ownerCollection.Id });
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
    }
}