using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;
using M101DotNet.WebApp.Models;
using M101DotNet.WebApp.Models.Home;
using MongoDB.Bson;
using System.Linq.Expressions;

namespace M101DotNet.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var blogContext = new BlogContext();

            var recentPosts = blogContext.Posts.Find(new BsonDocument())
                              .SortByDescending(x => x.Id)
                              .Limit(10)
                              .ToListAsync()
                              .Result;

            var model = new IndexModel
            {
                RecentPosts = recentPosts
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult NewPost()
        {
            return View(new NewPostModel());
        }

        [HttpPost]
        public async Task<ActionResult> NewPost(NewPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var blogContext = new BlogContext();

            var post = new Post()
            {
                Author = User.Identity.Name,
                Title = model.Title,
                Content = model.Content,
                CreatedAtUtc = DateTime.UtcNow,
                Tags = model.Tags.Split(',').ToList(),
                Comments = new List<Comment>()
            };

            await blogContext.Posts.InsertOneAsync(post);

            return RedirectToAction("Post", new { id = post.Id });
        }

        [HttpGet]
        public async Task<ActionResult> Post(string id)
        {
            var blogContext = new BlogContext();

            var post = blogContext.Posts.Find(x => x.Id == ObjectId.Parse(id)).Limit(1).FirstOrDefaultAsync().Result;

            if (post == null)
            {
                return RedirectToAction("Index");
            }

            var model = new PostModel
            {
                Post = post
            };

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Posts(string tag = null)
        {
            var blogContext = new BlogContext();

            var posts = blogContext.Posts.Find(x => x.Tags.Contains(tag))
                                         .SortByDescending(x => x.CreatedAtUtc)
                                         .ToListAsync()
                                         .Result;

            if (posts.Count == 0)
            {
                posts = blogContext.Posts.Find(new BsonDocument())
                                         .SortByDescending(x => x.CreatedAtUtc)
                                         .ToListAsync()
                                         .Result;
            }

            return View(posts);
        }

        [HttpPost]
        public async Task<ActionResult> NewComment(NewCommentModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Post", new { id = model.PostId });
            }

            var blogContext = new BlogContext();

            var comment = new Comment()
            {
                Author = User.Identity.Name,
                Content = model.Content,
                CreatedAtUtc = DateTime.UtcNow
            };

            await blogContext.Posts.UpdateOneAsync(x => x.Id == ObjectId.Parse(model.PostId), 
                new BsonDocument("$push", new BsonDocument("Comments", comment.ToBsonDocument())));

            return RedirectToAction("Post", new { id = model.PostId });
        }
    }
}