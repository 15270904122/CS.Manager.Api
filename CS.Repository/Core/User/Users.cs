using CS.Repository.Core.Attributes;
using CS.Repository.Core.Entity;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Repository.Core.User
{
    [ManagerIdentity]
    public class Users : AuditEntity<long>
    {
        [Column(DbType = "varchar(50) NOT NULL")]
        public string Name { get; set; }

        [Column(DbType = "varchar(50) NOT NULL")]
        public string UserName { get; set; }

        [Column(DbType = "varchar(500) NOT NULL")]
        public string Password { get; set; }

        [Column(DbType = "varchar(50) NOT NULL")]
        public string Email { get; set; }

        [Column(DbType = "varchar(50) NOT NULL")]
        public string Phone { get; set; }

        [Column(DbType = "varchar(50) NOT NULL")]
        public string IdCard { get; set; }

        [Column(IsVersion = true)]
        public int Version { get; set; }
    }
}
