﻿using System.Collections.Generic;
using System.Linq;
using Dal.Repository;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class QuestionAnswerRepository : BaseRepository<QuestionAnswer>, IQuestionAnswerRepository
    {
        public QuestionAnswerRepository(WriterContext db) : base(db)
        {
        }

        public List<QuestionAnswer> GetByQuestionVisibleIf(Question question)
        {
            return Entity.Where(qa => qa.AffectVisibilityOfQuestions.Any(q => q.Id == question.Id)).ToList();
        }

        public override QuestionAnswer Save(QuestionAnswer model)
        {
            model.Question = model.Question.AttachIfNot(Db);
            return base.Save(model);
        }
    }
}