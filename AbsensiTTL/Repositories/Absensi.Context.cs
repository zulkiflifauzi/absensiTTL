﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AbsensiTTL.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AbsensiEntities : DbContext
    {
        public AbsensiEntities()
            : base("name=AbsensiEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<ACGroup> ACGroups { get; set; }
        public DbSet<ACTimeZone> ACTimeZones { get; set; }
        public DbSet<ACUnlockComb> ACUnlockCombs { get; set; }
        public DbSet<AlarmLog> AlarmLogs { get; set; }
        public DbSet<AttParam> AttParams { get; set; }
        public DbSet<AuditedExc> AuditedExcs { get; set; }
        public DbSet<AUTHDEVICE> AUTHDEVICEs { get; set; }
        public DbSet<CHECKEXACT> CHECKEXACTs { get; set; }
        public DbSet<CHECKINOUT> CHECKINOUTs { get; set; }
        public DbSet<DEPARTMENT> DEPARTMENTS { get; set; }
        public DbSet<DeptUsedSch> DeptUsedSchs { get; set; }
        public DbSet<FaceTemp> FaceTemps { get; set; }
        public DbSet<HOLIDAY> HOLIDAYS { get; set; }
        public DbSet<LeaveClass> LeaveClasses { get; set; }
        public DbSet<LeaveClass1> LeaveClass1 { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<NUM_RUN> NUM_RUN { get; set; }
        public DbSet<NUM_RUN_DEIL> NUM_RUN_DEIL { get; set; }
        public DbSet<ReportItem> ReportItems { get; set; }
        public DbSet<SchClass> SchClasses { get; set; }
        public DbSet<SECURITYDETAIL> SECURITYDETAILS { get; set; }
        public DbSet<SHIFT> SHIFTs { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }
        public DbSet<TBSMSALLOT> TBSMSALLOTs { get; set; }
        public DbSet<TBSMSINFO> TBSMSINFOes { get; set; }
        public DbSet<TEMPLATE> TEMPLATEs { get; set; }
        public DbSet<USER_OF_RUN> USER_OF_RUN { get; set; }
        public DbSet<USER_SPEDAY> USER_SPEDAY { get; set; }
        public DbSet<USER_TEMP_SCH> USER_TEMP_SCH { get; set; }
        public DbSet<UserACMachine> UserACMachines { get; set; }
        public DbSet<UserACPrivilege> UserACPrivileges { get; set; }
        public DbSet<USERINFO> USERINFOes { get; set; }
        public DbSet<USER> USERS { get; set; }
        public DbSet<UserUpdate> UserUpdates { get; set; }
        public DbSet<UserUsedSClass> UserUsedSClasses { get; set; }
        public DbSet<EmOpLog> EmOpLogs { get; set; }
        public DbSet<ServerLog> ServerLogs { get; set; }
        public DbSet<UsersMachine> UsersMachines { get; set; }
        public DbSet<ViewCheckInOut> ViewCheckInOuts { get; set; }
    }
}
