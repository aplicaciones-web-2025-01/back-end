using System;
using System.Collections.Generic;
using learning_center_back.Shared.Domain.Model.Entities;

namespace learning_center_back.Tutorials.Domain.Models.Entities
{
    public class Book : BaseEntity
    {
        public Book(string name, string description, DateTime publishDate, int points)
        {
            Name = name;
            Description = description;
            PublishDate = publishDate;
            Points = points;
            IsActive = true;
            CreatedDate = DateTime.UtcNow;
            Chapters = new List<Chapter>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public int Points { get; set; }
        public List<Chapter> Chapters { get; } = new();

        public Category Category { get; set; }
        public int? CategoryId { get; set; }
    }


}