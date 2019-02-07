using Caldwell.Infrastructure.Models;
using Caldwell.Infrastructure.Repository;
using System;

namespace Caldwell.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // System.Console.WriteLine("Hello World!");

            var repo = new GenericRepository<Catalog>();
            var list = repo.GetAll();
        }
    }
}
