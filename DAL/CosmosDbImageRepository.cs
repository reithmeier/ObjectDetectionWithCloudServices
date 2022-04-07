using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.Azure.Cosmos;
using System.Configuration;

namespace DAL {
  public class CosmosDbImageRepository : IImageRepository {
    /// The Azure Cosmos DB endpoint
    private string _endpointUrl = ConfigurationManager.AppSettings["EndpointUrl"]; 

    /// The primary key for the Azure DocumentDB account
    private string _primaryKey = ConfigurationManager.AppSettings["PrimaryKey"]; 

    // The Cosmos client instance
    private CosmosClient cosmosClient;

    // The database we will create
    private Database database;

    // The container we will create.
    private Container container;

    // The name of the database and container we will create
    private string databaseId = "CLC3ProjectDatabase";
    private string containerId = "ImageContainer";


    public CosmosDbImageRepository() {
      this.cosmosClient = new CosmosClient(_endpointUrl, _primaryKey);

      var createDb = CreateDatabaseAsync();
      createDb.GetAwaiter().GetResult();
      var createContainer = CreateContainerAsync();
      createContainer.GetAwaiter().GetResult();
    }


    private async Task CreateDatabaseAsync() {
      this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
    }

    private async Task CreateContainerAsync() {
      this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/Name");
    }


    public async Task<IEnumerable<Image>> FindAllAsync() {
      const string sqlQueryText = "SELECT * FROM c";

      var queryDefinition = new QueryDefinition(sqlQueryText);
      var queryResultSetIterator = this.container.GetItemQueryIterator<Image>(queryDefinition);
      var items = new List<Image>();

      while (queryResultSetIterator.HasMoreResults) {
        var currentResultSet = await queryResultSetIterator.ReadNextAsync();
        items.AddRange(currentResultSet);
      }

      return items;
    }

    public async Task<Image> FindByIdAsync(string id) {
      const string sqlQueryText = "SELECT * FROM c WHERE c.id = @id";


      QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
      queryDefinition.WithParameter("@id", id);
      var queryResultSetIterator = this.container.GetItemQueryIterator<Image>(queryDefinition);

      var items = new List<Image>();

      while (queryResultSetIterator.HasMoreResults) {
        var currentResultSet = await queryResultSetIterator.ReadNextAsync();
        items.AddRange(currentResultSet);
      }

      return items.Count > 0 ? items[0] : null;
    }

    public async Task<string> InsertAsync(Image data) {
      if (data.Id == null) throw new ArgumentNullException(nameof(data), "data.Id must not be null");
      var response = await this.container.CreateItemAsync<Image>(data, new PartitionKey(data.Name));
      return response.Resource.Id;
    }

    public async Task<string> UpsertAsync(Image data)
    {
      var response = await this.container.UpsertItemAsync(data, new PartitionKey(data.Name));
      return response.Resource.Id;
    }
  }
}