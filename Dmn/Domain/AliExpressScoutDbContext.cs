using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Categories;
using Domain.Entities.Productions;
using Domain.Entities.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace UnitOfWork
{
    public class AliExpressScoutDbContext : DbContext
    {
        public AliExpressScoutDbContext(DbContextOptions<AliExpressScoutDbContext> options)
           : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Production>(b =>
            {
                b.HasOne<Category>(p => p.Category).WithMany().HasForeignKey(p => p.CategoryId);
            });


            modelBuilder.Entity<Query>().HasData(
                new Query(Guid.Parse("6c02b5ad-00c3-4f43-b262-bf3a023a795b"),
                "Array.from(document.getElementsByClassName('categories-list-box')[0].getElementsByTagName('a')).map(item => {return item.href})",
                "Get category urls from main page",
                QueryType.CategoryUrlList,
                true));
            modelBuilder.Entity<Query>().HasData(
                new Query(Guid.Parse("a364e8aa-155c-4467-b9af-1c2d6bae19ad"),
                "Array.from(document.getElementsByClassName('categories-list-box')[0].getElementsByTagName('a')).map(item => {return item.innerHTML})",
                "Get category names from main page",
                QueryType.CategoryNameList,
                true));
            modelBuilder.Entity<Query>().HasData(
                new Query(Guid.Parse("6ba88697-1977-4e1e-90fb-27acbacf5985"),
                "Array.from(document.getElementsByClassName('categories-list-box')[0].getElementsByTagName('a')).map(item => {return item.innerHTML})",
                "Get product lists from secified category page",
                QueryType.ProductList,
                true));
            modelBuilder.Entity<Query>().HasData(
                new Query(Guid.Parse("2194ab2e-001e-4a24-a41e-7fd46d72c08c"),
                "var records = Array.from(document.getElementsByTagName('li'));var paginationElement = records.filter(element => element.parentElement && element.parentElement.children[0].children[0].tagName === 'SPAN' );paginationElement[paginationElement.length - 1].click();paginationElement.map(item => item.innerHTML)",
                "Click next page in specified category page",
                QueryType.ProductListNextPage,
                true));
            modelBuilder.Entity<Query>().HasData(
                new Query(Guid.Parse("e0492bd1-a2d4-4080-84ab-af1e5220ef61"),
                "var records = Array.from(document.getElementsByTagName('li'));var paginationElement = records.filter(element => element.parentElement && element.parentElement.children[0].children[0].tagName === 'SPAN' );paginationElement[paginationElement.length - 1].classList.value",
                "Check last page in specified category with using css class",
                QueryType.ProductLastPage,
                true));

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Production> Productions { get; set; }
        public DbSet<Query> Queries { get; set; }
    }
}
