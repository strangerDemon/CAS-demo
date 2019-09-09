using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZoneTop.Application.SSO.Common.Entity
{
    /// <summary>
    /// ���ݿ� �û���¼��
    /// </summary>
    [Table("T_SsoCasSession")]
    public class CasLogEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Key]
        public string CasLogId { get; set; }
        /// <summary>
        /// �û�ID
        /// </summary>
        /// <returns></returns>
        public string UserId { get; set; }
        /// <summary>
        /// �û��˺�
        /// </summary>
        /// <returns></returns>
        public string UserAccount { get; set; }
        /// <summary>
        /// �û�����
        /// </summary>
        /// <returns></returns>
        public string UserName { get; set; }
        /// <summary>
        /// ��¼IP
        /// </summary>
        /// <returns></returns>
        public string IPAddress { get; set; }
        /// <summary>
        /// ��¼ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? LoginTime { get; set; }
        /// <summary>
        /// �ǳ�ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? LogoutTime { get; set; }
        /// <summary>
        /// ��¼״̬ 0 �ǳ���1��¼
        /// </summary>
        /// <returns></returns>
        public int? LogStatus { get; set; }
        /// <summary>
        /// ���������
        /// </summary>
        /// <returns></returns>
        public string BrowserType { get; set; }
        /// <summary>
        /// SESSIONID
        /// </summary>
        /// <returns></returns>
        public string SESSIONID { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        public string Description { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public void Create()
        {
            this.CasLogId = Guid.NewGuid().ToString();
            this.LoginTime = DateTime.Now;
            this.LogStatus = 1;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(string keyValue)
        {
            this.CasLogId = keyValue;
        }
        #endregion
    }
}