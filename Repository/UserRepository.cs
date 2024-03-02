namespace Comment_Post.Repository
{
    

    using System;
    using System.Collections.Generic;
    using System.Linq;
  
    using Dapper;
    using Microsoft.EntityFrameworkCore;
    using global::Comment_Post.Data;
    using global::Comment_Post.Models;

    namespace Comment_Post.Repositories
    {
        public class UserRepository
        {
            private readonly ApplicationDbContext _context;

            public UserRepository(ApplicationDbContext context)
            {
                _context = context;
            }

            public List<AppUser> GetUsersWithPostsLast30Days()
            {
             
                var usersWithPosts = _context.Users
                    .Include(u => u.Posts)
                    .Where(u => u.Posts.Any(p => p.CreatedAt >= DateTime.Now.AddDays(-30)))
                    .ToList();

                return usersWithPosts;
            }

            public List<AppUser> GetUsersWithPostsLast30DaysDapper()
            {
                
                using var connection = _context.Database.GetDbConnection();
                connection.Open();

                var query = @"
                SELECT DISTINCT u.*
                FROM AspNetUsers u
                INNER JOIN posts p ON u.Id = p.UserId
               ";
             //   WHERE p.CreatedAt >= @StartDate
                var usersWithPosts = connection.Query<AppUser>(query, new { StartDate = DateTime.Now.AddDays(-30) }).ToList();

                return usersWithPosts;
            }
        }
    }


}
