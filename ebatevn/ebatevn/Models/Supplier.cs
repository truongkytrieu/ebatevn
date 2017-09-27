using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ebatevn.Models
{
    public class Supplier
    {
        [BsonId]
        // Mvc don't know how to create ObjectId from string
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int SupplierId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int Test { get; set; }

        [Required]
        public string Language { get; set; }
    }
}
