using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common;

public static class Setting
{
    public const string _exchangefanout = "fanout_exchange";
    public const string _queueNamefanout = "fanout_queue";

    public const string _exchangeheader = "header_exchange";
    public const string _queueNameheader = "header_queue";

    public const string _exchangetopic = "topic_exchange";
    public const string _queueNametopic = "topic_queue";
    public const string _bindingKeytopic_all = "testtopic.*";
    public const string _bindingKeytopic_add = "testtopic.add";


    public const string _exchangeDirect = "Direct_exchange";
    public const string _queueNameDirect = "Direct_queue";

}
