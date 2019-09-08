using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Find;
using EPiServer.Find.Statistics;
using Geta.EPi.Extensions.Sample.Controllers;
using Geta.EPi.Extensions.Sample.Models.ViewModels;
using System.Web.Mvc;

namespace Geta.EPi.Extensions.Sample.Controllers
{
    public class GDPRApiDemoPageController : PageControllerBase<GDPRApiDemoPage>
    {
        private readonly IStatisticsClient _statisticsClient;
        private readonly ITrackSanitizer _trackSanitizer;
        private readonly ITrackSanitizerPatternRepository _trackSanitizerPatternRepository;

        public GDPRApiDemoPageController(IClient searchClient)
        {
            _statisticsClient = searchClient.Statistics();
            _trackSanitizer = searchClient.TrackSanitizer();
            _trackSanitizerPatternRepository = _trackSanitizer.TrackSaniziterRepository;
        }

        [ValidateInput(false)]
        public ViewResult Index(GDPRApiDemoPage currentPage, string q)
        {
            var model = new GDPRDemoModel(currentPage);

            if (!string.IsNullOrWhiteSpace(q))
            {
                var getResult = _statisticsClient.StatisticsGetGDPR(q);
                model.GetHits = getResult.Hits;
            }

            var gdprPattern = _trackSanitizerPatternRepository.GetAll();

            model.PlainTextFilterPatterns = gdprPattern?.Where(t => t.PatternType == TrackSanitizerFilterType.PlainText).Select(i => i.PatternString).ToList();
            model.RegexFilterPatterns = gdprPattern?.Where(t => t.PatternType == TrackSanitizerFilterType.Regex).Select(i => i.PatternString).ToList();
            model.WildcardFilterPatterns = gdprPattern?.Where(t => t.PatternType == TrackSanitizerFilterType.Wildcard).Select(i => i.PatternString).ToList();

            return View("Index", model);
        }

        [HttpPost, ActionName("Index")]
        [ValidateInput(false)]
        public ViewResult Delete(GDPRApiDemoPage currentPage, string q)
        {
            var model = new GDPRDemoModel(currentPage);
            if (!string.IsNullOrWhiteSpace(q))
            {
                var deleteResult = _statisticsClient.StatisticsDeleteGDPR(q);
                model.DeleteStatus = deleteResult.Status;
            }
            else
            {
                model.DeleteStatus = "Statistics is GDPR compliance.";
            }

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public void Save(GDPRApiDemoPage currentPage, string plaintextFilter, string wildcardFilter, string regexFilter)
        {
            _trackSanitizerPatternRepository.DeleteAll();

            var filterPatterns = new List<TrackSanitizerPattern>();
            var plaintextPatterns = string.IsNullOrEmpty(plaintextFilter)
                ? new List<TrackSanitizerPattern>()
                : plaintextFilter.Split(new[] {"\r\n"}, StringSplitOptions.None)
                    .Where(i => !string.IsNullOrEmpty(i.Trim()))
                    .Select(t => new TrackSanitizerPattern {PatternString = t, PatternType = TrackSanitizerFilterType.PlainText});
            var wildcardPatterns = string.IsNullOrEmpty(wildcardFilter)
                ? new List<TrackSanitizerPattern>()
                : wildcardFilter.Split(new[] {"\r\n"}, StringSplitOptions.None)
                    .Where(i => !string.IsNullOrEmpty(i.Trim()))
                    .Select(t => new TrackSanitizerPattern {PatternString = t, PatternType = TrackSanitizerFilterType.Wildcard});
            var regexPatterns = string.IsNullOrEmpty(regexFilter)
                ? new List<TrackSanitizerPattern>()
                : regexFilter.Split(new[] {"\r\n"}, StringSplitOptions.None)
                    .Where(i => !string.IsNullOrEmpty(i.Trim()))
                    .Select(t => new TrackSanitizerPattern {PatternString = t, PatternType = TrackSanitizerFilterType.Regex});

            filterPatterns.AddRange(plaintextPatterns);
            filterPatterns.AddRange(wildcardPatterns);
            filterPatterns.AddRange(regexPatterns);

            _trackSanitizerPatternRepository.Add(filterPatterns);
        }
    }
}
