using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using MrCMS.Entities.Documents.Web.FormProperties;
using MrCMS.Settings;

namespace MrCMS.Shortcodes.Forms
{
    public class RadioButtonListRenderer : IFormElementRenderer<RadioButtonList>
    {
        public TagBuilder AppendElement(RadioButtonList formProperty, string existingValue,
            FormRenderingType formRenderingType)
        {
            var values = existingValue == null
                ? new List<string>()
                : existingValue.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToList();

            var tagBuilder = new TagBuilder("div");
            foreach (var checkbox in formProperty.Options)
            {
                var cbLabelBuilder = new TagBuilder("label");
                cbLabelBuilder.Attributes["for"] =
                    TagBuilder.CreateSanitizedId(formProperty.Name + "-" + checkbox.Value, "-");
                cbLabelBuilder.AddCssClass("radio");

                var radioButtonBuilder = new TagBuilder("input");
                radioButtonBuilder.Attributes["type"] = "radio";
                radioButtonBuilder.Attributes["value"] = checkbox.Value;
                radioButtonBuilder.AddCssClass(formProperty.CssClass);

                if (existingValue != null)
                {
                    if (values.Contains(checkbox.Value))
                        radioButtonBuilder.Attributes["checked"] = "checked";
                }
                else if (checkbox.Selected)
                {
                    radioButtonBuilder.Attributes["checked"] = "checked";
                }

                if (formProperty.Required)
                {
                    radioButtonBuilder.Attributes["data-val"] = "true";
                    radioButtonBuilder.Attributes["data-val-required"] =
                        string.Format("The field {0} is required",
                            string.IsNullOrWhiteSpace(formProperty.LabelText)
                                ? formProperty.Name
                                : formProperty.LabelText);
                }

                radioButtonBuilder.Attributes["name"] = formProperty.Name;
                radioButtonBuilder.Attributes["id"] =
                    TagBuilder.CreateSanitizedId(formProperty.Name + "-" + checkbox.Value, "-");
                cbLabelBuilder.InnerHtml.AppendHtml(radioButtonBuilder + checkbox.Value);
                if (formRenderingType == FormRenderingType.Bootstrap3)
                {
                    var radioContainer = new TagBuilder("div");
                    radioContainer.AddCssClass("radio");
                    radioContainer.InnerHtml.AppendHtml(cbLabelBuilder);
                    tagBuilder.InnerHtml.AppendHtml(radioContainer);
                }
                else
                {
                    tagBuilder.InnerHtml.AppendHtml(cbLabelBuilder);
                }
            }

            return tagBuilder;
        }

        public TagBuilder AppendElement(FormProperty formProperty, string existingValue,
            FormRenderingType formRenderingType)
        {
            return AppendElement(formProperty as RadioButtonList, existingValue, formRenderingType);
        }

        public bool IsSelfClosing => false;
    }
}