using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BooksAPI.Models
{
    [ModelMetadataType(typeof(MetaData))]
    public partial class ResponseModel : Users
    {
        public ResponseModel(Users user)
        {

            this.UserId = user.UserId;
            this.Email = user.Email;
            this.Name = user.Name;
            this.Contact = user.Contact;
            this.Gender = user.Gender;
            this.AddressLine = user.AddressLine;
            this.CityId = user.CityId;
            this.StateId = user.StateId;
        }

        public string AccessToken { get; set; }
        public string RefreshToken { set; get; }
    }
}
