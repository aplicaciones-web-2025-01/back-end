using System;
using learning_center_back.Shared.Domain.Model.Entities;

namespace learning_center_back.Tutorials.Domain.Models.Entities
{
    public class Chapter : BaseEntity
    {
        public Chapter() { }

        public Chapter(string title, int number, string content, Book book)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Number = number;
            Content = content ?? throw new ArgumentNullException(nameof(content));
            Book = book ?? throw new ArgumentNullException(nameof(book));
        }

        public string Title { get; init; }
        public int Number { get; init; }
        public string Content { get; init; }
        public Book Book { get; init; }
    }

}