﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.Installation
{
    public partial interface IUpgradeService
    {
        string DatabaseVersion();
        void UpgradeData(string fromversion, string toversion);
    }
}
