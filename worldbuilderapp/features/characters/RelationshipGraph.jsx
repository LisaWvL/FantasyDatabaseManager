//TODO this is a component that enables a visual graph display of related entities of the type CharacterRelationship
//TODO this component will be named RelationshipGraph
//TODO this component will be a functional component
//TODO this component will be created in the components folder
//TODO this component will be exported as the default export
//TODO this component will import React
//TODO this component will import the react-graph-vis library
//TODO this component will import the CharacterRelationship model
//TODO this component will have a div element with the id "relationship-graph"
//TODO this component will have a useEffect hook that will run once on mount
//TODO the useEffect hook will fetch all CharacterRelationship entities from the database
//TODO the useEffect hook will create a new graph object
//TODO the graph object will have nodes and edges properties
//TODO the nodes property will be an array of objects with id and label properties
//TODO the edges property will be an array of objects with from, to, and label properties
//TODO the graph object will be passed to the Graph component from the react-graph-vis library
//TODO the Graph component will be rendered inside the div element with the id "relationship-graph"
//TODO the Graph component will have the graph object as a prop
//TODO the Graph component will have options with the following properties:
//TODO layout: { hierarchical: true }
//TODO edges: { arrows: { to: { enabled: true } } }
//TODO nodes: { shape: "box" }
//TODO the Graph component will have events with the following properties:
//TODO select: function that logs the selected node to the console
//TODO the Graph component will have a style prop with the following properties:
//TODO width: "100%"
//TODO height: "600px"
//TODO this component will return the JSX code
//TODO this component will be imported in the App.js file
//TODO this component will be rendered in the main content area of the application
//TODO this component will display a visual graph of the relationships between characters
//TODO this component will allow users to view and interact with the relationships between characters
//TODO this component will be used to visualize the connections between characters in the worldbuilder application
//TODO this component will be used to navigate and explore the relationships between characters in the worldbuilder application

//TODO Add methods and functionality to the RelationshipGraph component
//TODO Add a chapter toggle to the RelationshipGraph component
//TODO Add relationship type filtering to the RelationshipGraph component
//TODO Add mirrored relationship support to the RelationshipGraph component
//TODO Enable right-click to edit or remove relationship in the RelationshipGraph component
//TODO Add a "Create New Relationship" button to the RelationshipGraph component
//TODO Add a "Copy Relationship" button to the RelationshipGraph component
//TODO Add a "Show All Related" button to the RelationshipGraph component
//TODO Add a "Copy to Chapter" button to the RelationshipGraph component
//TODO Add a "Pin" button to the RelationshipGraph component
//TODO Add a "Show PlotPoints for Current Chapter Only" toggle to the RelationshipGraph component
//TODO Add a "Show All Chapters" toggle to the RelationshipGraph component

import React, { useEffect } from 'react';
import { Graph } from 'react-graph-vis';
import CharacterRelationship from '../models/CharacterRelationship';

export default function RelationshipGraph() {
  useEffect(() => {
    CharacterRelationship.find().then((relationships) => {
      const nodes = relationships.map((r) => ({ id: r.id, label: r.name }));
      const edges = relationships.map((r) => ({
        from: r.character1Id,
        to: r.character2Id,
        label: r.relationshipType,
      }));
      const graph = { nodes, edges };
      const options = {
        layout: { hierarchical: true },
        edges: { arrows: { to: { enabled: true } } },
        nodes: { shape: 'box' },
      };
      const events = {
        select: function (event) {
          console.log(event.nodes);
        },
      };
      const style = { width: '100%', height: '600px' };
      const graphComponent = (
        <Graph graph={graph} options={options} events={events} style={style} />
      );
      document.getElementById('relationship-graph').appendChild(graphComponent);
    });
  }, []);
  return <div id="relationship-graph"></div>;
}

//RelationshipGraph.jsx
//Add: chapter toggle
//Add: relationship type filtering
// Add: mirrored relationship support
// Enable right - click to edit or remove relationship
// Add: �Create New Relationship� button
// Add: �Copy Relationship� button
// Add: �Show All Related� button
