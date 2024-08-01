﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using LAPTemplateMVC.Models;
using LAPTemplateMVC.Models.dboSchema;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace LAPTemplateMVC.Models
{
    public partial interface IFinanzRechnerContextProcedures
    {
        Task<int> AusgabeDeleteAsync(long? AUSGABEID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<AusgabeGetAllResult>> AusgabeGetAllAsync(short? VALID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<AusgabeGetByAusgabeIDResult>> AusgabeGetByAusgabeIDAsync(long? AUSGABEID, short? VALID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<AusgabeGetByKategorieIDResult>> AusgabeGetByKategorieIDAsync(long? KATEGORIEID, short? VALID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<int> AusgabeInsertAsync(long? KATEGORIEID, string BEZEICHNUNG, decimal? BETRAG, short? VALID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<int> AusgabeUpdateAsync(long? AUSGABEID, long? KATEGORIEID, string BEZEICHNUNG, decimal? BETRAG, short? VALID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<int> EinnahmeDeleteAsync(long? EINNAHMEID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<EinnahmeGetAllResult>> EinnahmeGetAllAsync(short? VALID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<EinnahmeGetByEinnahmeIDResult>> EinnahmeGetByEinnahmeIDAsync(long? EINNAHMEID, short? VALID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<EinnahmeGetByKategorieIDResult>> EinnahmeGetByKategorieIDAsync(long? KATEGORIEID, short? VALID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<int> EinnahmeInsertAsync(long? KATEGORIEID, string BEZEICHNUNG, decimal? BETRAG, short? VALID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<int> EinnahmeUpdateAsync(long? EINNAHMEID, long? KATEGORIEID, string BEZEICHNUNG, decimal? BETRAG, short? VALID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<int> KategorieDeleteAsync(long? KATEGORIEID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<KategorieGetAllResult>> KategorieGetAllAsync(short? VALID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<KategorieGetByKategorieIDResult>> KategorieGetByKategorieIDAsync(long? KATEGORIEID, short? VALID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<int> KategorieInsertAsync(string NAME, string FARBE, short? VALID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<int> KategorieUpdateAsync(long? KATEGORIEID, string NAME, string FARBE, short? VALID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
    }
}
