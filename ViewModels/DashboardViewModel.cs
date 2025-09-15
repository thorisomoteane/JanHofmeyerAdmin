namespace JanHofmeyerAdmin.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalProjects { get; set; }
        public int ActiveEvents { get; set; }
        public int PendingReviews { get; set; }
        public int NewMessages { get; set; }
        public int PendingVolunteers { get; set; }
        public int TotalGalleryItems { get; set; }
        public int RecentRegistrations { get; set; }
        public decimal TotalDonationsThisMonth { get; set; } = 0;
    }
}