// ***********************************************************
// FTP协议工作模式枚举
// Creator:YangMingkun  Date:2009-6-22
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;

namespace Windy.Common.Libraries.Ftp
{
    /// <summary>
    /// FTP协议模式
    /// </summary>
    public enum FtpMode
    {
        /// <summary>
        /// 主动模式
        /// </summary>
        PORT,
        /// <summary>
        /// 被动模式
        /// </summary>
        PASV
    }
}
