using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CM_APPLICATIONS.Models
{
    public enum ParserState
    {
        /// <summary>
        ///   Indicates that the parser has no datasource and is not properly setup.
        /// </summary>
        NoDataSource = 0,
        /// <summary>
        ///   Indicates that the parser is ready to begin parsing.
        /// </summary>
        Ready = 1,
        /// <summary>
        ///   Indicates that the parser is currently parsing the datasource.
        /// </summary>
        Parsing = 2,
        /// <summary>
        ///   Indicates that the parser has finished parsing the datasource.
        /// </summary>
        Finished = 3
    }
}