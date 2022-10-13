using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlaySafe.Data;
using PlaySafe.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Win32;

namespace PlaySafe.Controllers
{
    public class usersController : Controller
    {
        private readonly dbContext _context;
        private readonly IWebHostEnvironment _hostingEnvirnoment;

        public usersController(dbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvirnoment = hostingEnvironment;
            _context = context;
        }

        // GET: users
        public async Task<IActionResult> Index()
        {
            var typeIsUser = _context.userType.Where(x => x.usersType == "player").FirstOrDefault();
            var userType = HttpContext.Session.GetString("userType");


            if (userType == "Admin") typeIsUser = _context.userType.Where(x => x.usersType == "Guard").FirstOrDefault();

            else if (userType == "Owner") typeIsUser = _context.userType.Where(x => x.usersType == "Admin").FirstOrDefault();

            else if (userType == "Guard") typeIsUser = _context.userType.Where(x => x.usersType == "Player").FirstOrDefault();

            else return Redirect("/Home/Index");
            var dbContext = _context.user.Where(u => u.userTypeId == typeIsUser.id);
            return View(await dbContext.ToListAsync());
        }

        // GET: users/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.user == null)
            {
                return NotFound();
            }

            var user = await _context.user
                .Include(u => u.userType)
                .FirstOrDefaultAsync(m => m.id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: users/Create
        public IActionResult Create()
        {
            ViewData["userTypeId"] = new SelectList(_context.Set<userType>(), "id", "id");
            return View();
        }

        // POST: users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("name,userName,password,phoneNum,confirmPassword,photo")] registerViewModel register)
        {
            if (ModelState.IsValid)
            {
                var userNameCheck = _context.user.Where(x => x.userName == register.userName).FirstOrDefault();
                if(userNameCheck != null)
                {
                    string error = register.userName + " is already taken";
                    ModelState.AddModelError("userName", error);
                    return View(register);
                }
               if(register.password != register.confirmPassword)
                {
                    ModelState.AddModelError("confirmPassword", "Passwords don't match");
                    return View(register);
                }
                var typeIsUser = _context.userType.Where(x => x.usersType == "player").FirstOrDefault();
                if(typeIsUser != null)
                {
                    var userType = HttpContext.Session.GetString("userType");
                    

                    if (userType == "Admin") typeIsUser = _context.userType.Where(x => x.usersType == "Guard").FirstOrDefault();

                    else if (userType == "Owner") typeIsUser = _context.userType.Where(x => x.usersType == "Admin").FirstOrDefault();

                    else if (userType == "Guard") typeIsUser = _context.userType.Where(x => x.usersType == "Player").FirstOrDefault();

                    else return Redirect("/Home/Index");

                    Guid id = Guid.NewGuid();
                    string filename = null;
                    if (register.photo != null)
                    {
                        id = Guid.NewGuid();
                        string filePath = "images\\" + id.ToString("D");
                        string path = Path.Combine(_hostingEnvirnoment.WebRootPath, filePath);                                              
                        if(!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                            filename = register.photo.FileName;
                            register.photo.CopyTo(new FileStream(Path.Combine(path,filename), FileMode.Create));
                        }
                        else
                        {
                            //overWrite Image
                        }
                            
                    }
                    var passwordHash = GetHash(register.password);
                    user User = new user()
                    {
                        id = id,
                        userTypeId = typeIsUser.id,
                        name = register.name,
                        userName = register.userName,
                        password = passwordHash,
                        createdDate = DateTime.Now,
                        phoneNum = register.phoneNum,
                        points = 0,
                        photo = filename
                    };
                    _context.user.Add(User);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(register);
        }
        // GET: users/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {

            if (id == null || _context.user == null)
            {
                return NotFound();
            }

            var user = await _context.user.FindAsync(id);
            var register = new registerViewModel()
            {
                name = user.name,
                userName = user.userName,
                password = "",
                confirmPassword = "",
                phoneNum = user.phoneNum,
            };
            if (user == null)
            {
                return NotFound();
            }
            return View(register);
        }

        // POST: users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("name,userName,password,confirmPassword,phoneNum")] registerViewModel user)
        {
            if (ModelState.IsValid)
            {
                var userNameCheck = _context.user.Where(x => x.userName == user.userName && x.id != id).FirstOrDefault();
                if (userNameCheck != null)
                {
                    string error = user.userName + " is already taken";
                    ModelState.AddModelError("userName", error);
                    return View(user);
                }
                if (user.password != user.confirmPassword)
                {
                    ModelState.AddModelError("confirmPassword", "Passwords don't match");
                    return View(user);
                }
                try
                {
                    user User = _context.user.Find(id);
                    User.name = user.name;
                    User.userName = user.userName;
                    User.password = GetHash(user.password);
                    User.phoneNum = user.phoneNum;
                    _context.user.Update(User);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!userExists(new Guid(HttpContext.Session.GetString("userId"))))
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
            return View(user);
        }

        // GET: users/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.user == null)
            {
                return NotFound();
            }

            var user = await _context.user
                .Include(u => u.userType)
                .FirstOrDefaultAsync(m => m.id == id);
            if (user == null)
            {
                return Redirect("/Home/index");
            }

            return View(user);
        }

        // POST: users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.user == null)
            {
                return Problem("Entity set 'dbContext.user'  is null.");
            }
            var user = await _context.user.FindAsync(id);
            if (user != null)
            {
                _context.user.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool userExists(Guid id)
        {
          return _context.user.Any(e => e.id == id);
        }
        public async Task<IActionResult> login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> login([Bind("userName,password")] loginViewModel loginCredentials)
        {
            var passwordHash = GetHash(loginCredentials.password);
            var user = _context.user.Where(x => x.userName == loginCredentials.userName && x.password == passwordHash).FirstOrDefault();

            if(user != null)
            {
                var theUserType = _context.userType.Where(x => x.id == user.userTypeId).FirstOrDefault();
                
                if (theUserType == null)
                {
                    return View();//return error
                }
                else
                {
                HttpContext.Session.SetString("userId", user.id.ToString("D"));
                HttpContext.Session.SetString("userTypeId", user.userTypeId.ToString("D"));
                HttpContext.Session.SetString("userType", theUserType.usersType.ToString());
                }   

                return Redirect("/Home/Index");
            }
            ModelState.AddModelError("userName", "Username or password are invalid");
            return View(loginCredentials);
        }
        public async Task<IActionResult> ChooseMatch()
        {
            var costs = _context.entry.ToArray();
            var points = _context.user.Where(x => x.id == new Guid(HttpContext.Session.GetString("userId"))).FirstOrDefault();
            List<int> allCosts = new List<int>();
            foreach(var entry in costs)
            {
                allCosts.Add(entry.price);
            }
            allCosts.Sort();
            ViewBag.costs = allCosts;
            ViewBag.Points = points.points;
            ViewBag.userId = HttpContext.Session.GetString("userId");
            ViewBag.typeId = HttpContext.Session.GetString("userTypeId");
            ViewBag.userType = HttpContext.Session.GetString("userType");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChooseMatch([Bind("matchCost,withPoints")] matchViewModel match)
        {
           
            var costs = _context.entry.ToArray();
            List<int> allCosts = new List<int>();
            foreach (var entry2 in costs)
            {
                allCosts.Add(entry2.price);
            }
            allCosts.Sort();
            ViewBag.costs = allCosts;
            var userId = HttpContext.Session.GetString("userId");
            var userGuid = new Guid(userId);
            var user = _context.user.Where(x => x.id == userGuid).FirstOrDefault();
            var entry = _context.entry.Where(n => n.price == match.matchCost).FirstOrDefault();
            if(user == null || userId == null || entry == null)
            {
                return View("error");//need to change
            }
            var oldMatches = _context.matchHistory.Where(x => x.userId == userGuid).ToArray();
            
            matchHistory lastMatch = _context.matchHistory.Where(n => n.userId == userGuid && n.active == true).FirstOrDefault();           
            if (lastMatch == null || lastMatch.createdDate.AddHours(24) <= DateTime.Now)
             {
                if(lastMatch != null)
                {
                    lastMatch.active = false;
                }
                
                matchHistory matchHistory = new matchHistory()
                {
                     id = Guid.NewGuid(),
                     userId = userGuid,
                     entryId = entry.id,                     
                     createdDate = DateTime.Now,
                     active = true,
                     withPoints = match.withPoints
                 };
                 _context.matchHistory.Add(matchHistory);
                if (match.withPoints)
                {
                    if(oldMatches.Count() <= 5)
                    {
                        ModelState.AddModelError("matchCost", "Cannot use Points unless you play 5 matches");
                        ViewBag.Points = user.points;
                        return View();
                    }
                    if(user.points < match.matchCost)
                    {
                        ModelState.AddModelError("matchCost", "You don't have enough points");
                        ViewBag.Points = user.points;
                        return View();
                    }
                        user.points = user.points - match.matchCost;
                }
                else
                {
                    user.points = user.points + (match.matchCost * 4);
                }
               
                ViewBag.Points = user.points;
                _context.user.Update(user);
                 _context.SaveChanges();
                
                //return Redirect("/Users/logOut");                
                return View();
             }
            ViewBag.Points = user.points;
            ModelState.AddModelError("matchCost", "You Have to until for your next match");
            return View();
        }
        public byte[] GetHash(string PasswordHash)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(PasswordHash));
        }
        public bool VerifyPasswordHash(string password, byte[] passwordHash)
        {
            return passwordHash.SequenceEqual(GetHash(password));
        }
        public async Task<IActionResult> logOut()
        {
            HttpContext.Session.Clear();
            return Redirect("/Users/login"); 
        }

        public IActionResult comment()
        {
            var userid = HttpContext.Session.GetString("userId");
            var userInGuid = new Guid(userid);
            var user = _context.user.Where(x => x.id == userInGuid).FirstOrDefault();
            HttpContext.Session.SetString("photo", (user.photo));
            return View();
        }


        [HttpPost]
        public IActionResult comment(string commentText)
        {

            var userid = HttpContext.Session.GetString("userId");
            var userInGuid = new Guid(userid);
            if (userInGuid == Guid.Empty)
            {
                return RedirectToAction("login");

            }
           
            comments c = new comments()
            {
                id = Guid.NewGuid(),
                comment = commentText,
                userId = userInGuid
            };
            _context.comments.Add(c);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult readcomment()
        {
            var c = _context.comments.Include(m => m.comment).ToList();
            return View(c);
        }

        public IActionResult chatting()
        {
            var userid = HttpContext.Session.GetString("userId");
            var userInGuid = new Guid(userid);
            var user = _context.user.Where(x => x.id == userInGuid).FirstOrDefault();            
            HttpContext.Session.SetString("Name", user.name.ToString());
            return View();
        }

    }
}
