using System.Collections.Generic;
using Nop.Core.Domain.Localization;
using MongoDB.Bson.Serialization.Attributes;

namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// Represents a product tag
    /// </summary>
    [BsonIgnoreExtraElements]
    public partial class ProductTag : BaseEntity, ILocalizedEntity
    {
        public ProductTag()
        {
            Locales = new List<LocalizedProperty>();
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the count
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets product id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the collection of locales
        /// </summary>
        public IList<LocalizedProperty> Locales { get; set; }
    }
}
