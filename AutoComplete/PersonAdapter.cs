using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace AutoComplete
{
    public class PersonAdapter : ArrayAdapter<Person>
    {
        public List<Person> Items, TempItems, FilteredItems;
        private PersonFilter filter;

        public PersonAdapter(Context context, List<Person> objects) : base(context, Resource.Layout.support_simple_spinner_dropdown_item, objects)
        {
            Items = objects;
            TempItems = new List<Person>(objects);
            FilteredItems = new List<Person>(objects);
            filter = new PersonFilter(this);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Person person = GetItem(position);
            if (convertView == null)
            {
                LayoutInflater inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
                convertView = inflater.Inflate(Resource.Layout.row_item, parent, false);
            }

            TextView txtName = convertView.FindViewById<TextView>(Resource.Id.textViewName);
            TextView txtEmail = convertView.FindViewById<TextView>(Resource.Id.textViewEmail);

            if (txtName != null)
                txtName.Text = person.Name;

            if (txtEmail != null)
                txtEmail.Text = person.Email;

            return convertView;
        }

        public override Filter Filter => this.filter;

    }

    public class PersonFilter : Filter
    {
        PersonAdapter adapter;

        public PersonFilter(PersonAdapter adapter)
        {
            this.adapter = adapter;
        }

        public override ICharSequence ConvertResultToStringFormatted(Java.Lang.Object resultValue)
        {
            if (resultValue != null)
            {
                Person person = resultValue as Person;
                return new Java.Lang.String(person.Name);
            }
            else
            {
                return new Java.Lang.String();
            }
        }

        protected override FilterResults PerformFiltering(ICharSequence constraint)
        {
            if (constraint != null)
            {
                adapter.FilteredItems.Clear();
                foreach (Person person in adapter.TempItems)
                {
                    if (person.Name.ToLower().StartsWith(constraint.ToString().ToLower()))
                    {
                        adapter.FilteredItems.Add(person);
                    }
                }

                FilterResults filterResults = new FilterResults();
                filterResults.Values = FromArray(adapter.FilteredItems.ToArray());
                filterResults.Count = adapter.FilteredItems.Count;
                return filterResults;
            }
            else
            {
                return new FilterResults();
            }
        }

        protected override void PublishResults(ICharSequence constraint, FilterResults results)
        {
            if (results != null && results.Count > 0)
            {
                Person[] persons = results.Values.ToArray<Person>();
                this.adapter.Clear();
                foreach (Person person in persons)
                {
                    adapter.Add(person);
                    adapter.NotifyDataSetChanged();
                }
            }
            else
            {
                adapter.Clear();
                adapter.NotifyDataSetChanged();
            }
        }
    }
}
