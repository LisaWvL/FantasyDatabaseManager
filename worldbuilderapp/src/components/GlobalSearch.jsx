//TODO this compoenent will be a search bar that will search through all the data in the database and return the results in a list
//TODO this component will be used in the header of the application
//TODO this component will have a text input field and a search button
//TODO this component will have a list of search results
//TODO this component will be styled with Bootstrap
//TODO this component will be a functional component
//TODO this component will be named GlobalSearch
//TODO this component will be created in the components folder
//TODO this component will be exported as the default export
//TODO this component will import React
//TODO this component will have a div with a class of container
//TODO this component will have an h1 element with the text "Global Search"
//TODO this component will have an input element with the following attributes:
//TODO type="text"
//TODO className="form-control"
//TODO placeholder="Search..."
//TODO this component will have a button element with the following attributes:
//TODO className="btn btn-primary mt-2"
//TODO text "Search"
//TODO this component will have an hr element
//TODO this component will have an h2 element with the text "Search Results"
//TODO this component will have a ul element with three li elements with the text "Result 1", "Result 2", and "Result 3"
//TODO this component will return the JSX code
//TODO this component will be imported in the App.js file
//TODO this component will be rendered in the Header component
import React from 'react';

export default function GlobalSearch() {
  return (
    <div className="container">
      <h1>Global Search</h1>
      <input type="text" className="form-control" placeholder="Search..." />
      <button className="btn btn-primary mt-2">Search</button>
      <hr />
      <h2>Search Results</h2>
      <ul>
        <li>Result 1</li>
        <li>Result 2</li>
        <li>Result 3</li>
      </ul>
    </div>
  );
}
