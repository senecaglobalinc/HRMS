using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Entities
{
    public class Addendum : BaseEntity
    {
        /// <summary>
        /// AddendumId
        /// </summary>
        public int AddendumId
        {
            get;
            set;
        }

        /// <summary>
        /// ProjectId
        /// </summary>
        public int ProjectId
        {
            get;
            set;
        }

        /// <summary>
        /// SOWId
        /// </summary>
        public string SOWId
        {
            get;
            set;
        }

        /// <summary>
        /// AddendumNo
        /// </summary>
        public string AddendumNo
        {
            get;
            set;
        }

        /// <summary>
        /// RecipientName
        /// </summary>
        public string RecipientName
        {
            get;
            set;
        }

        /// <summary>
        /// AddendumDate
        /// </summary>
        public DateTime AddendumDate
        {
            get;
            set;
        }

        /// <summary>
        /// Note
        /// </summary>
        public string Note
        {
            get;
            set;
        }

        /// <summary>
        /// Id
        /// </summary>
        public int Id
        {
            get;
            set;
        }


        /// <summary>
        /// IdNavigation
        /// </summary>
        //public virtual SOW IdNavigation
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Project
        /// </summary>
        public virtual Project Project
        {
            get;
            set;
        }
        public virtual SOW SOW
        {
            get;
            set;
        }
    }
}
