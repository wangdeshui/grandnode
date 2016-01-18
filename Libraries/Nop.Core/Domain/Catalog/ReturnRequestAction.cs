﻿using MongoDB.Bson.Serialization.Attributes;
using Nop.Core.Domain.Localization;
using System.Collections.Generic;

namespace Nop.Core.Domain.Orders
{
    /// <summary>
    /// Represents a return request action
    /// </summary>
    [BsonIgnoreExtraElements]
    public partial class ReturnRequestAction : BaseEntity, ILocalizedEntity
    {
        public ReturnRequestAction()
        {
            Locales = new List<LocalizedProperty>();
        }
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// Gets or sets the collection of locales
        /// </summary>
        public IList<LocalizedProperty> Locales { get; set; }
    }
}
