using System.Collections.Generic;
using System.Linq;
using Sunplus.Agent.Model.Definitions;

namespace Sunplus.Agent.Model.Base.Configurations
{
    public static class ProductManager
    {
        private static IEnumerable<ProductElement> _activePTLinkProducts;
        private static IEnumerable<ProductElement> _activeCommLinkProducts;
        private static IEnumerable<ProductElement> _activeReportProducts;
        private static IEnumerable<ProductElement> _activeReportSportIdProducts;
        private static IEnumerable<ProductElement> _newFlowProducts;

        public static IEnumerable<ProductElement> ActivePTLinkProducts
        {
            get
            {
                _activePTLinkProducts = _activePTLinkProducts ?? 
                    ProductElements.Where(p => p.Active && !string.IsNullOrEmpty(p.ViewPTLink)).ToList();

                return _activePTLinkProducts;
            }
        }

        public static IEnumerable<ProductElement> ActiveCommLinkProducts
        {
            get
            {
                _activeCommLinkProducts = _activeCommLinkProducts ??
                    ProductElements.Where(p => p.Active && !string.IsNullOrEmpty(p.ViewCommLink)).ToList();

                return _activeCommLinkProducts;
            }
        }

        public static IEnumerable<ProductElement> ActiveReportProducts
        {
            get
            {
                _activeReportProducts = _activeReportProducts ??
                    ProductElements.Where(p => p.Active && !string.IsNullOrEmpty(p.ReportSPParam)).ToList();

                return _activeReportProducts;
            }
        }

        public static IEnumerable<ProductElement> ActiveReportSportIdProducts
        {
            get
            {
                _activeReportSportIdProducts = _activeReportSportIdProducts ??
                    ProductElements.Where(p => p.Active && !string.IsNullOrEmpty(p.ReportSPParam) && p.SportId != -1).ToList();

                return _activeReportSportIdProducts;
            }
        }

        public static IEnumerable<ProductElement> NewFlowProducts
        {
            get
            {
                _newFlowProducts = _newFlowProducts ??
                    ProductElements.Where(p => p.Id >= ProductIdConstants.CockFighting).ToList();

                return _newFlowProducts;
            }
        }

        public static IEnumerable<ProductElement> ProductElements
        {
            get
            {
                return ProductSection.Instance.Products.Cast<ProductElement>();
            }
        }
    }
}
