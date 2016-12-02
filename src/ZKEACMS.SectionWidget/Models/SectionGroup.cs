/* http://www.zkea.net/ Copyright 2016 ZKEASOFT http://www.zkea.net/licenses */
using System;
using System.Collections.Generic;
using System.Linq;
using ZKEACMS.SectionWidget.Service;
using Easy.Extend;
using Easy.MetaData;
using Easy.Models;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.DependencyInjection;

namespace ZKEACMS.SectionWidget.Models
{
    [ViewConfigure(typeof(SectionGroupMetaData)), Table("SectionGroup")]
    public class SectionGroup : EditorEntity
    {
        public int? ID { get; set; }
        public string GroupName { get; set; }
        public string SectionWidgetId { get; set; }
        public string PartialView { get; set; }
        public int? Order { get; set; }
        public string PercentWidth { get; set; }
        [NotMapped]
        public bool IsLoadDefaultData { get; set; }
        [NotMapped]
        public IEnumerable<SectionContent> SectionContents { get; set; }

        private T GetContent<T>(SectionContentBase.Types type) where T : SectionContent
        {
            if (SectionContents != null)
            {
                return SectionContents.FirstOrDefault(m => m != null && m.SectionContentType == (int)type) as T;
            }
            return null;
        }

        private IEnumerable<T> GetContents<T>(SectionContentBase.Types type) where T : SectionContent
        {
            if (SectionContents != null)
            {
                return SectionContents.Where(m => m != null && m.SectionContentType == (int)type).Cast<T>();
            }
            return null;
        }
        [NotMapped]
        public SectionContentTitle SectionTitle
        {
            get
            {
                return GetContent<SectionContentTitle>(SectionContentBase.Types.Title);
            }
        }
        [NotMapped]
        public IEnumerable<SectionContentTitle> SectionTitles
        {
            get
            {
                return GetContents<SectionContentTitle>(SectionContentBase.Types.Title);
            }
        }
        [NotMapped]
        public SectionContentCallToAction CallToAction
        {
            get
            {
                return GetContent<SectionContentCallToAction>(SectionContentBase.Types.CallToAction);
            }
        }
        [NotMapped]
        public IEnumerable<SectionContentCallToAction> CallToActions
        {
            get
            {
                return GetContents<SectionContentCallToAction>(SectionContentBase.Types.CallToAction);
            }
        }
        [NotMapped]
        public SectionContentImage SectionImage
        {
            get
            {
                return GetContent<SectionContentImage>(SectionContentBase.Types.Image);
            }
        }
        [NotMapped]
        public IEnumerable<SectionContentImage> SectionImages
        {
            get
            {
                return GetContents<SectionContentImage>(SectionContentBase.Types.Image);
            }
        }
        [NotMapped]
        public SectionContentParagraph Paragraph
        {
            get
            {
                return GetContent<SectionContentParagraph>(SectionContentBase.Types.Paragraph);
            }
        }
        [NotMapped]
        public IEnumerable<SectionContentParagraph> Paragraphs
        {
            get
            {
                return GetContents<SectionContentParagraph>(SectionContentBase.Types.Paragraph);
            }
        }

        private string _templateName;
        public string GetTemplateName()
        {
            if (_templateName.IsNullOrWhiteSpace())
            {
                var template = new Easy.ServiceLocator().Current.GetService<ISectionTemplateService>().Get(PartialView);
                if (template != null)
                {
                    _templateName = template.Title;
                }
                else
                {
                    _templateName = "Error";
                }
            }
            return _templateName;
        }
        [NotMapped]
        public override string Description { get; set; }
        [NotMapped]
        public override int? Status { get; set; }
        [NotMapped]
        public override string Title { get; set; }
    }

    class SectionGroupMetaData : ViewMetaData<SectionGroup>
    {
        protected override void ViewConfigure()
        {
            ViewConfig(m => m.GroupName).AsTextBox().Required();
            ViewConfig(m => m.ID).AsHidden();
            ViewConfig(m => m.SectionWidgetId).AsHidden();
            ViewConfig(m => m.IsLoadDefaultData).AsHidden().Ignore();
            ViewConfig(m => m.PartialView).AsDropDownList().DataSource(() =>
            {
                return new Easy.ServiceLocator().Current.GetService<ISectionTemplateService>()
                    .GetAll()
                    .ToDictionary(m => m.TemplateName, m => m.Title);
            });
            ViewConfig(m => m.Title).AsHidden();
            ViewConfig(m => m.Description).AsHidden();
        }
    }
}