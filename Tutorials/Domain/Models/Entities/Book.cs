using System;
using System.Collections.Generic;
using learning_center_back.Shared.Domain.Model.Entities;

namespace learning_center_back.Tutorials.Domain.Models.Entities
{
    public class Book : BaseEntity
    {
        public Book(string name, string description, DateTime publishDate, int points)
        {
            Name = name ;
            Description = description ;
            PublishDate = publishDate;
            Points = points;
            IsActive = true;
            CreatedDate = DateTime.UtcNow;
            Categories = new List<Category>();
        }

        public string Name { get; init; }
        public string Description { get; init; }
        public DateTime PublishDate { get; init; }
        public int Points { get; init; }
        public List<Category> Categories { get; } = new List<Category>();
    }
}