using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Windy.Common.Libraries
{
    public struct ExecuteResult
    {

        /// <summary>
        /// 正常(0)
        /// </summary>
        
        public const short OK = 0;

        /// <summary>
        /// 参数错误(1)
        /// </summary>
        public const short PARAM_ERROR = 1;

        /// <summary>
        /// 数据库访问错误(2)
        /// </summary>
        public const short ACCESS_ERROR = 2;

        /// <summary>
        /// 接口内部异常(3)
        /// </summary>
        public const short EXCEPTION = 3;

        /// <summary>
        /// 资源未发现(4)
        /// </summary>
        public const short RES_NO_FOUND = 4;

        /// <summary>
        /// 资源已经存在(5)
        /// </summary>
        public const short RES_IS_EXIST = 5;

        /// <summary>
        /// 文件格式错误
        /// </summary>
        public const short FILE_FORMAT_ERROR = 6;

        /// <summary>
        /// 其他错误(9)
        /// </summary>
        public const short OTHER_ERROR = 9;

        public static string GetResultMessage(short shRet)
        {
            string msg = string.Empty;
            switch (shRet)
            {
                case ExecuteResult.OK:
                    msg = "成功";
                    break;
                case ExecuteResult.PARAM_ERROR:
                    msg = "参数错误";
                    break;
                case ExecuteResult.EXCEPTION:
                    msg = "接口内部异常";
                    break;
                case ExecuteResult.RES_NO_FOUND:
                    msg = "资源未发现";
                    break;
                case ExecuteResult.RES_IS_EXIST:
                    msg = "资源已经存在";
                    break;
                case ExecuteResult.FILE_FORMAT_ERROR:
                    msg = "文件格式错误";
                    break;
                case ExecuteResult.OTHER_ERROR:
                    msg = "其他错误";
                    break;
                default:
                    break;
            }
            return msg;
        }
    }
}
