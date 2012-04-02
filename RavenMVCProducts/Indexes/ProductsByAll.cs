using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using RavenMVCProducts.Models;

namespace RavenMVCProducts.Indexes
{
    using Raven.Abstractions.Indexing;
    using Raven.Client.Indexes;

    public class Indexes
    {
        // How to create an RavenDB index from code
        public class ProductsByAll : AbstractIndexCreationTask<Product>
        {
            public ProductsByAll()
            {
                Map = prods => from p in prods
                               select new
                               {
                                   p.Name,
                                   p.CategoryId,
                                   p.SupplierId,
                                   p.Code,
                                   p.ListPrice
                               };

                Index(x => x.Name, FieldIndexing.Analyzed);
                Index(x => x.Code, FieldIndexing.Analyzed);
            }
        }

        public class ProductsByDiscontinuedAndListPriceSortByListPrice : AbstractIndexCreationTask<Product>
        {
            public ProductsByDiscontinuedAndListPriceSortByListPrice()
            {
                Map = docs => from doc in docs
                              select new
                              {
                                  doc.Discontinued,
                                  doc.ListPrice
                              };
            }
        }
    }
}