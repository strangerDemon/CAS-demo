using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZoneTop.Application.SSO.Common.Entity
{
    /// <summary>
    /// ���ݿ� �ͻ��˵�¼��־��
    /// </summary>
    [Table("T_SsoAppSession")]
    public class ClientLogEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Key]
        public string AppLogId { get; set; }
        /// <summary>
        /// CAS�ỰID
        /// </summary>
        /// <returns></returns>
        public string CasLogId { get; set; }
        /// <summary>
        /// Ӧ��ID
        /// </summary>
        /// <returns></returns>
        public string AppId { get; set; }
        /// <summary>
        /// Ӧ������
        /// </summary>
        /// <returns></returns>
        public string AppName { get; set; }
        /// <summary>
        /// Ӧ�õ�ַ
        /// </summary>
        /// <returns></returns>
        public string AppUrl { get; set; }
        /// <summary>
        /// Ʊ��
        /// </summary>
        /// <returns></returns>
        public string ST { get; set; }
        /// <summary>
        /// Ʊ�ݴ���ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? STCreateTime { get; set; }
        /// <summary>
        /// Ʊ����Чʱ��������Ϊ��λ
        /// </summary>
        /// <returns></returns>
        public int? STEFTime { get; set; }
        /// <summary>
        /// Ʊ����֤ -1 δ��֤ 0 ʧ�� 1 �ɹ� 
        /// </summary>
        /// <returns></returns>
        public int? STValidated { get; set; }
        /// <summary>
        /// Ʊ����֤ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime? STValidateTime { get; set; }
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
        /// ��ע
        /// </summary>
        /// <returns></returns>
        public string Description { get; set; }
        /// <summary>
        /// SESSIONID
        /// </summary>
        /// <returns></returns>
        public string SESSIONID { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public void Create()
        {
            this.AppLogId = Guid.NewGuid().ToString();
            this.LogStatus = 1;
            this.LoginTime = DateTime.Now;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(string keyValue)
        {
            this.AppLogId = keyValue;
        }
        #endregion
    }
}