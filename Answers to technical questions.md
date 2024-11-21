q:1 
I spent approximately 6 hours on the coding assignment. If I had more time, I would enhance the solution by focusing on the following areas:

Adding Logging: Implementing a comprehensive logging mechanism to capture important application events and assist in troubleshooting and monitoring.

Improving Security Measures: Currently, the application lacks any form of login or authentication to secure user data and access. To address this, I would implement robust authentication and authorization mechanisms, such as token-based authentication (e.g., JWT) or OAuth, to restrict access and ensure data security. Additionally, I would review the application for potential vulnerabilities, like securing API endpoints, sanitizing inputs to prevent injection attacks, and enforcing HTTPS communication.

Enhancing Error Handling: Adding detailed error handling to manage unexpected scenarios gracefully and provide meaningful feedback to users.

UI Visual Enhancements: Improving the user interface to ensure a seamless and visually appealing user experience.

By focusing on these areas, particularly security, I would aim to make the application more reliable, robust, and safe for users.


Q:2 What was the most useful feature that was added to the latest version of your language of choice?
Please include a snippet of code that shows how you've used it


Raw String Literals
Simplifies working with multi-line strings, JSON, or regular expressions by removing the need for escape characters.

string json = """
{
    "name": "John",
    "age": 30
}
""";

Q:3 How would you track down a performance issue in production? 

Application Insights
database logs
Azure Alerts



Have you ever had to do this?

Yes, I encountered a performance issue in the SynerScope application when processing a large volume of documents with many pages. For example, handling approximately 16,000 files with 70,000 pages would normally take between 2.5 to 4 hours. However, scaling up to 10 times (approximately 160,000 files with 700,000 pages) that volume caused the process to take around 80 hours, which was not acceptable.

Our application uses a background processing system powered by Azure Batch, similar to Docker. By analyzing the database logs for each service, we found that the services themselves were executing efficiently. However, after further investigation, we identified two bottlenecks:

Message Queue Bottleneck:
All background services sent success or failure messages to a single message queue, which processed them one by one. This serialized processing created a bottleneck. To resolve this, we implemented a fix to process messages in batches of 32, significantly improving performance and reducing overall processing time.

Inefficient Database Design:
The system's database was inherited from an older design with features we no longer used, which introduced unnecessary complexity. For example, retrieving files related to a project required joining more than three tables, which slowed down scheduling for upcoming tasks, especially with large files. To address this, we simplified the database schema by consolidating relationships into a single table. This made queries faster and the system easier to maintain.

These optimizations drastically improved processing efficiency and allowed the application to scale effectively for larger workloads.


Q:4 What was the latest technical book you have read or tech conference you have been to? What did you learn?


I find that I learn most effectively through watching Pluralsight courses, which provide a hands-on approach to acquiring technical skills. Recently, I’ve focused on the following topics:

Terraform:
I explored how Terraform can help manage infrastructure as code and facilitate a smoother transition between cloud providers, such as moving from Azure to AWS. This knowledge is invaluable for ensuring portability and maintaining consistency across multi-cloud environments.

RabbitMQ:
I studied RabbitMQ as an alternative messaging service outside of the Microsoft stack. This expanded my understanding of distributed messaging systems and gave me insights into how to use RabbitMQ for reliable communication between microservices in various architectures.

React.js Hooks:
I deepened my knowledge of React.js by learning about React Hooks, including useState, useEffect, and custom hooks. This understanding proved highly beneficial in my last job, where I applied these concepts to build more modular and maintainable front-end components.

These courses have enhanced my ability to work with diverse technologies, making me more adaptable to different environments and challenges.


Q:5 What do you think about this technical assessment?


I found the technical assessment both challenging and rewarding. One of the key challenges was working with the Exchange Rates API (https://exchangeratesapi.io) and its limitations. The free version only allows EUR as the base currency, which required me to implement a custom converter to transition from a base of EUR to BTC. This solution was based on the assumption that the exchange rates for buying and selling were the same, which added an additional consideration when designing the logic.

The CoinMarketCap API (https://coinmarketcap.com/api) also posed a challenge due to its restriction of converting to only one currency type per request. This required making multiple API calls to handle all the necessary conversions, adding to the complexity of the task.

Despite these challenges, it was an enjoyable and educational experience. I appreciated the opportunity to work with real-world APIs, think critically about their limitations, and implement creative solutions to address them. It was both a technical challenge and a chance to learn something new.


Q:6 Please, describe yourself using JSON.

{
  "personalDetails": {
    "name": "Mounir Aziz",
    "location": "Egypt",
    "physicalDescription": {
      "eyeColor": "brown",
      "hairColor": "black",
      "height": "average"  
    },
    "profession": ".NET Engineer",
    "personalityTraits": [
      "ambitious",
      "dedicated",
      "problem-solver",
      "team-oriented"
    ],
    "skills": [
      "Mentoring",
      "Analytical Thinking",
      "Planning",
      "Programming"
    ],
    "hobbies": [
      "mentoring junior developers",
      "learning new technologies",
      "exploring advanced software solutions"
    ],
    "languages": ["Arabic (fluent)", "English (fluent)"]
  }
}

