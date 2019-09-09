namespace ZoneTop.Application.SSO.Common.Model
{
    /// <summary>
    /// HttpParameter 的摘要说明
    /// </summary>
    public class HttpModel
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        public HttpModel(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}={1}", Name, Value.ToString());
        }
    }
}