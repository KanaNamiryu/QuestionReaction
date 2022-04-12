using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionReaction.Services.CustomAnnotation
{
    /// <summary>
    /// DataAnnotaion custom pour qu'une liste de string doive retourner un nombre d'elements non null minimum
    /// </summary>
    public class MinCount : ValidationAttribute
    {
        private readonly int _minElements;
        /// <summary>
        /// constructeur de la classe
        /// </summary>
        /// <param name="minElements">nombre d'element minimum que doit contenir la classe</param>
        public MinCount(int minElements)
        {
            _minElements = minElements;
        }

        /// <summary>
        /// test le validité de la liste par rapport au critere en entrée du constucteur
        /// </summary>
        /// <param name="value">liste</param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            var iList = value as IList;
            var list = iList as List<string>;
            var notNullList = list
                .Where(c => c != null)
                .ToList();

            if (notNullList != null)
            {
                return notNullList.Count >= _minElements;
            }
            return false;
        }
    }
}
