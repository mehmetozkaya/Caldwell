using Caldwell.Core.Repository;
using Caldwell.Infrastructure.Crawler;
using Caldwell.Infrastructure.Models;
using Caldwell.Infrastructure.Repository;
using System;
using System.Threading.Tasks;

namespace Caldwell.Console
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CaldwellCrawler crawler = new CaldwellCrawler(CrawlerConsts.CrawleUrl)
                                    .AddDownloader();

                crawler.ReasonToSolve();

                //CatalogCreator creator = new CatalogCreator();
                //Task create = creator.Create();
                //create.Wait();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }

    public class CatalogCreator
    {
        private readonly IGenericRepository<Catalog> _repository;

        public CatalogCreator()
        {
            _repository = new GenericRepository<Catalog>();
        }

        public async Task Create()
        {
            var catalog = GetNewCatalog();
            await _repository.CreateAsync(catalog);
        }

        public Catalog GetNewCatalog()
        {

            var item = new Catalog()
            {
                CatalogTypeId = 1,
                CatalogBrandId = 1,
                Name = "IPhone Y",
                Slug = "iphone-Y",
                Star = 4.4,
                Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                Summary = "A seasonal delight we offer every autumn.  Pumpking bread with just a bit of spice, cream cheese frosting with just a hint of home.",
                Price = 19.5M,
                PictureUri = "product-52.png",

                AlfonsoPoint = "96",
                VersusPoint = "9.7",
                AntutuPoint = "6.4",
                Battery = "3000 Mah",
                Camera = "12 MP",
                Screen = "6.4 Inc",
                Storage = "64 GB",
                Ram = "4 GB",
                Cpu = "2.9 Ghz"
            };

            var newFeature = new Features()
            {
                Name = "Screen"
            };

            newFeature.FeatureItem.Add(new FeatureItem
            {
                Name = "Screen Width",
                Value = "5.45 Inc"
            });

            item.Features.Add(newFeature);

            return item;
        }
    }
}
