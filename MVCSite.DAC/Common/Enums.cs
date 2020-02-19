using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCSite.DAC.Common
{
    public enum NotificationEmailType
    {
        Never=0,
        Periodically=1,
        Instantly=2,
    }

    public enum CardStatus
    {
        Created = 1,
        Updated,
    }
}
