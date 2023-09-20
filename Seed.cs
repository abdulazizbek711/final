using ReviewApp.Data;
using ReviewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReviewApp
{
    public class Seed
    {
        private readonly DataContext dataContext;

        public Seed(DataContext context)
        {
            this.dataContext = context;
        }
        
        public void SeedDataContext()
{
    if (!dataContext.Reviews.Any())
    {
        // Create Pieces
        var pieces = new List<Piece>
        {
            new Piece { Name = "The Shawshank Redemption"
                , Type = "Movie" },
            new Piece { Name = "The Legend of Zelda: Breath of the Wild", Type = "Game" },
            new Piece { Name = "To Kill a Mockingbird", Type = "Book" },
          
        };
        dataContext.Pieces.AddRange(pieces);
        dataContext.SaveChanges();

        // Create Users
        var users = new List<User>
        {
            new User { UserName = "Jack",
                Email = "jack.cap@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("zvdhghdvvv1v1vy2gy1g21")},
            new User { UserName = "hunterzolomon",
                Email = "hunter007@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("y1g87t87s")},
            new User { UserName = "John",
                Email = "john.doe@gmail..com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123") },
            // Add more users as needed
        };
        dataContext.Users.AddRange(users);
        dataContext.SaveChanges();

        // Now, create Reviews with valid PieceId and UserId
        var reviews = new List<Review>()
        {
            new Review
            {
                ReviewName = "Movie Review",
                Title = "The Shawshank Redemption",
                Content = "The Shawshank Redemption is a timeless classic...",
                Rating = 9,
                UserId = users[0].UserId, // Use a valid user ID from the created users
                PieceId = pieces[0].PieceId, // Use a valid piece ID from the created pieces
            },
            new Review
            {
                ReviewName = "Book Review",
                Title = "To Kill a Mockingbird",
                Content = "To Kill a Mockingbird is a powerful novel...",
                Rating = 8,
                UserId = users[1].UserId, // Use a valid user ID from the created users
                PieceId = pieces[1].PieceId, // Use a valid piece ID from the created pieces
            },
            new Review
            {
                ReviewName = "Game Review",
                Title = "The Legend of Zelda: Breath of the Wild",
                Content = "Breath of the Wild is a breathtaking adventure game...",
                Rating = 10,
                UserId = users[2].UserId, // Use a valid user ID from the created users
                PieceId = pieces[2].PieceId, // Use a valid piece ID from the created pieces
            },
            // Add more reviews as needed
        };

        dataContext.Reviews.AddRange(reviews);
        dataContext.SaveChanges();
    }
}


        

              
        
    }
}

