﻿using Jbs.Yukari.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jbs.Yukari.Domain
{
    public class PersonTransformer
    {
        public static string[] GetUserTypes()
        {
            return ["user"];
        }

        public static string[] GetGroupTypes()
        {
            return [];
        }

        public User[] Transform(Person person)
        {
            return null;
        }
    }
}
