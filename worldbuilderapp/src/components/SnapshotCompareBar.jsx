//TODO implement a reusable component that compares snapshot versions of entities and displays the differences
//TODO the component should be named SnapshotCompareBar
//TODO the component should be a functional component
//TODO the component should be created in the components folder
//TODO the component should be exported as the default export
//TODO the component should accept two props: snapshot1 and snapshot2
//TODO the snapshot1 prop should be an object representing the first snapshot
//TODO the snapshot2 prop should be an object representing the second snapshot
//TODO the component should render a div element with the class "snapshot-compare-bar"
//TODO the div element should contain two div elements with the class "snapshot-compare-item"
//TODO the first div element should display the snapshot1 version number
//TODO the second div element should display the snapshot2 version number
//TODO the component should compare the two snapshots and display the differences
//TODO the component should highlight the differences between the two snapshots
//TODO the component should be styled with CSS
//TODO the component should be used to compare snapshots of entities in the worldbuilder application
//TODO the component should be used to track changes and updates to entities over time
//TODO the component should be used to visualize the differences between two versions of an entity
//TODO the component should be used to compare changes made to entities in different snapshots
//TODO the component should be used to identify and resolve conflicts between different versions of an entity

//Compare this snippet from worldbuilderapp / src / components / GlobalSearch.jsx:
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
import React from "react";

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
