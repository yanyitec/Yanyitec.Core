using System;
using System.Collections.Generic;
using System.Text;

namespace Yanyitec
{
    public enum HttpMethods
    {
        UNKNOWN,
        /// <summary>
        /// The OPTIONS method represents a request for information about the communication options available on the request/response chain identified by the Request-URI 
        /// OPTIONS方法用于获取指定URI请求或响应通信链上的有关通信选项信息。
        /// </summary>
        OPTIONS,
        /// <summary>
        /// The GET method means retrieve whatever information (in the form of an entity) is identified by the Request-URI 
        /// GET方法用于检索或获取服务器上的指定的资源。
        /// </summary>
        GET,
        /// <summary>
        /// The HEAD method is identical to GET except that the server MUST NOT return a message-body in the response. 
        /// HEAD方法和GET方法的区别在于，请求的响应中不保护消息体，只返回请求响应头。
        /// </summary>
        HEAD,
        /// <summary>
        /// The POST method is used to request that the origin server accept the entity enclosed in the request as a new subordinate of the resource identified by the Request-URI in the Request-Line. 
        /// POST方法用于请求源服务器接收到的请求实体中URL指定资源标识的下一级资源。 
        /// 相当于请求一个更具体的数据，比如说，你的URI代表了资源的一个集合，POST就会根据请求体中的数据去取这个集合中的数据／部分数据。
        /// </summary>
        POST,
        /// <summary>
        /// The PUT method requests that the enclosed entity be stored under the supplied Request-URI. 
        /// PUT方法用于通过指定的URI去存储请求中携带的数据
        /// </summary>
        PUT,
        /// <summary>
        /// The DELETE method requests that the origin server delete the resource identified by the Request-URI. 
        /// DELETE方法用于删除服务器指定URI的资源。
        /// </summary>
        DELETE,
        /// <summary>
        /// The TRACE method is used to invoke a remote, application-layer loop- back of the request message. 
        /// TRACE方法用于诊断指定的URI请求原始报文是否被修改，或损坏。
        /// </summary>
        TRACE

        
    }
}
