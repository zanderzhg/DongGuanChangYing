using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.Model
{
    public class M_AccessControllerAuthorization_Info
    {
        public string Sn { get; set; }
        //public string IpAddress { get; set; }
        public string AuthorizeDoors { get; private set; }

        public bool IsValidValue()
        {
            if (string.IsNullOrEmpty(Sn))
                return false;
            //if (string.IsNullOrEmpty(IpAddress))
            //    return false;
            if (string.IsNullOrEmpty(AuthorizeDoors))
                return false;

            return true;
        }

        public void AddDoors(int doorCount)
        {
            AuthorizeDoors = AuthorizeDoors.Equals("") ? doorCount.ToString() : AuthorizeDoors + "," + doorCount.ToString();
        }
    }
}
