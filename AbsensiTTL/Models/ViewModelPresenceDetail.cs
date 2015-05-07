using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace AbsensiTTL.Models
{
    public class ViewModelPresenceDetail
    {
        public int UserId { get; set; }

        public string NIK { get; set; }

        public string Name { get; set; }

        public string Jabatan { get; set; }

        public int JumlahMasuk { get; set; }

        public int Telat { get; set; }

        [DisplayName("Pulang Cepat")]
        public int PulangCepat { get; set; }

        [DisplayName("Tidak Absen Datang")]
        public int TidakAbsenDatang { get; set; }

        [DisplayName("Tidak Absen Pulang")]
        public int TidakAbsenPulang { get; set; }

        public int Masuk { get; set; }

        [DisplayName("Tidak Masuk")]
        public int TidakMasuk { get; set; }

        [DisplayName("Masuk Hari Libur")]
        public int MasukHariLibur { get; set; }

        [DisplayName("Hari Kerja Bulan Ini")]
        public int HariKerja { get; set; }

        [DisplayName("Transport (Rp)")]
        public string BantuanTransport { get; set; }

        public double BantuanTransportCopy { get; set; }

        [DisplayName("Perjln Dinas")]
        public int Dinas { get; set; }
    }
}