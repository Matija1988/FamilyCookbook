﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Common
{
    public interface ISuccessResponses
    {
        StringBuilder EntityDeleted(string entityName);

        StringBuilder EntityCreated();

        StringBuilder EntityUpdated();
    }
}
