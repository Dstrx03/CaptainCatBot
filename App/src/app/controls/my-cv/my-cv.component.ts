import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-my-cv',
  templateUrl: './my-cv.component.html',
  styleUrls: ['./my-cv.component.scss']
})
export class MyCvComponent implements OnInit {

  cvData: CurriculumVitae;

  constructor() {
    this.cvData = new CurriculumVitae();

    this.cvData.title = '.NET/Angular Full Stack Developer'

    this.cvData.personalDetails = {
      imgSrc: 'https://lh3.googleusercontent.com/a-/AAuE7mDYfHY2gfj3rmxdBmUpd3bFFauBJtyADppnuLZ_BQ=s120-p-rw-no',
      fullName: 'Aleksey Mogilyov',
      dateOfBirth: null,
      city: null,
      email: 'dstrx03@gmail.com',
      contactPhone: null,
      telegram: null
    }

    this.cvData.skills = new Skills();
    this.cvData.skills.languages.push(new LanguageEntry("English", "Upper-Intermediate"));
    this.cvData.skills.languages.push(new LanguageEntry("Ukrainian", "Native speaker"));
    this.cvData.skills.languages.push(new LanguageEntry("Russian", "Native speaker"));
    this.cvData.skills.skills.push(new SkillEntry('.NET Framework', [
      'C#', 'ASP.NET MVC', 'ASP.NET Web API', 'ASP.NET Identity', 'Entity Framework', 'LINQ', 'SignalR', 'NUnit', 'StructureMap', 'Hangfire', 'log4net'
    ]));
    this.cvData.skills.skills.push(new SkillEntry('Web', [
      'JavaScript', 'TypeScript', 'AngularJS', 'AngularAgility', 'Angular2+', 'Angular Material', 'jQuery', 'RxJS', 'HTML5', 'CSS3', 'SCSS', 'Bootstrap'
    ]));
    this.cvData.skills.skills.push(new SkillEntry('Databases', [
      'SQL', 'T-SQL', 'Microsoft SQL Server', 'Stored procedures'
    ]));
    this.cvData.skills.skills.push(new SkillEntry('General', [
      'OOP', 'OOD', 'Design Patterns', 'MVC', 'Dependency Injection', 'Test-driven development', 'RESTful and SOAP Web Services', 'AJAX', 'GIT', 'SVN', 'IIS', 'Jenkins', 'Jira', 'Scrum'
    ]));

    this.cvData.workExperience = new WorkExperience; 
    this.cvData.workExperience.entries.push(new WorkExperienceEntry(
      'TurnKey Lender',
      'https://turnkey-lender.com',
      'Full Stack .NET Developer',
      new Date(2015, 3, 6),
      null,
      `End-to-end software development and support of the company's main product including custom and internal projects, in particular, 
      participating in development of financial calculations, decision-making systems, integrations with credit bureaus and payment services.
      Working in Agile Scrum methodology within a team and delivering accepted functionality in each sprint. 
      Environment: C#, ASP.NET MVC, ASP.NET Web API, Entity Framework, StructureMap, HTML5, CSS3, JavaScript, jQuery, Bootstrap, AngularJS, Microsoft SQL Server, IIS.`
    ));
    
    this.cvData.professionalSummary = new ProfessionalSummary();
    this.cvData.professionalSummary.p.push(`${this.cvData.workExperience.getTotalExperience()} of experience in designing, developing, and testing multi-tier applications using Microsoft .NET Framework, SQL Server, HTML/CSS/JS and various frameworks with Agile development methodology.`);
    this.cvData.professionalSummary.p.push(`Strong experience in designing and implementing scalable systems with OOP approach and Design Patterns, creating loosely coupled classes using Dependency Injection, Test-driven development with NUnit.`);
    this.cvData.professionalSummary.p.push(`Proficient in developing data layer using Entity Framework, LINQ, creating and consuming RESTful and SOAP Web Services, designing and implementing modern feel UI with HTML, CSS, JavaScript and related frameworks, including Single Page Applications with Angular.`);
    this.cvData.professionalSummary.p.push(`Extensive practice in bug tracking, issue tracking using Jira, Continuous Integration/Continuous Deployment tools like Jenkins, version control tools as GIT and SVN.`);
    this.cvData.professionalSummary.p.push(`Hands-on experience in Finance domain, in particular, successful development and support of numerous integrations with diverse credit bureaus and payment services.`);

    this.cvData.education = new Education();
    this.cvData.education.educationEntries.push(new EducationEntry(
      'Kharkiv National Automobile and Highway University',
      'Master\'s degree in Automobile Transport',
      new Date(2008, 0, 1),
      new Date(2013, 0 , 1)
    ));
    this.cvData.education.coursesEntries.push(new EducationEntry(
      'STEP Computer Academy',
      '\'Android Mobile Application Development\'',
      new Date(2014, 0, 1),
      new Date(2015, 0 , 1)
    ));

    this.cvData.portfolio = new Portfolio();
    this.cvData.portfolio.links.push(new PortfolioLink('Projects on GitHub', 'https://github.com/Dstrx03'));
    this.cvData.portfolio.links.push(new PortfolioLink('Apps on Google Play', 'https://play.google.com/store/apps/developer?id=Aleksey+Mogilyov'));
    this.cvData.portfolio.links.push(new PortfolioLink('YouTube channel', 'https://www.youtube.com/channel/UCOGgyRfHDttilrpEZnvhVOA'));
  }

  ngOnInit() {
  }

}



export class CurriculumVitae {
  personalDetails: PersonalDetails;
  title: string;
  professionalSummary: ProfessionalSummary;
  skills: Skills;
  workExperience: WorkExperience;
  education: Education;
  portfolio: Portfolio;
}

export class PersonalDetails {
  imgSrc: string;
  fullName: string;
  dateOfBirth: Date;
  city: string;
  email: string;
  contactPhone: string;
  telegram: string;
}

export class ProfessionalSummary {
  p: string[];

  constructor() {
    this.p = [];
  }
}

export class WorkExperience {
  entries: WorkExperienceEntry[];

  constructor() {
    this.entries = [];
  }

  getTotalExperience(): string {
    const startDate = this.entries.map(x => x.startDate).reduce(function (pre, cur) {
      return new Date(pre).getTime() > new Date(cur).getTime() ? cur : pre;
    });
    let endDate = this.entries.map(x => x.endDate).reduce(function (pre, cur) {
      return new Date(pre).getTime() > new Date(cur).getTime() ? cur : pre;
    });
    if (endDate === null) endDate = new Date();
    let diff = (endDate.getTime() - startDate.getTime()) / 1000;
    diff /= (60 * 60 * 24);
    const years = Math.floor(diff/365.25);
    return `${years}+ years`;
  };
}

export class WorkExperienceEntry {
  name: string;
  link: string;
  jobTitle: string;
  startDate: Date;
  endDate: Date;
  description: string;

  constructor(name: string,
    link: string,
    jobTitle: string,
    startDate: Date,
    endDate: Date,
    description: string) {
      this.name = name;
      this.link = link;
      this.jobTitle = jobTitle;
      this.startDate = startDate;
      this.endDate = endDate;
      this.description = description;
  }

  getExperience(): string {
    const endDate = this.endDate === null ? new Date() : this.endDate;
    let diff = (endDate.getTime() - this.startDate.getTime()) / 1000;
    diff /= (60 * 60 * 24);
    const years = Math.floor(diff/365.25);
    const months = Math.floor(diff/30.417) - (years * 12);
    return `${years} years ${months} months`;
  }
}

export class Education {
  educationEntries: EducationEntry[];
  coursesEntries: EducationEntry[];

  constructor() {
    this.educationEntries = [];
    this.coursesEntries = [];
  }
}

export class EducationEntry{
  name: string;
  details: string;
  startDate: Date;
  endDate: Date;

  constructor(name: string,
    details: string,
    startDate: Date,
    endDate: Date){
      this.name = name;
      this.details = details;
      this.startDate = startDate;
      this.endDate = endDate;
    }
}

export class Skills {
  languages: LanguageEntry[];
  skills: SkillEntry[];

  constructor() {
    this.languages = [];
    this.skills = [];
  }
}

export class LanguageEntry {
  name: string;
  level: string;

  constructor(name: string, level: string){
    this.name = name;
    this.level = level;
  }
}

export class SkillEntry {
  caption: string;
  skills: string[];

  constructor (caption: string, skills: string[]) {
    this.caption = caption;
    this.skills = skills;
  }
}

export class Portfolio {
  links: PortfolioLink[];

  constructor() {
    this.links = [];
  }
}

export class PortfolioLink {
  caption: string;
  href: string;

  constructor(caption: string, href: string){
    this.caption = caption;
    this.href = href;
  }
}
