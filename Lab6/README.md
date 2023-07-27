<h5> 1.	Define each HTTP Status Code that you used in this project and explain what each code means. </h5>
<ul>
	<li>
		200 ( OK ) : Indicates the request is succesful. It's used as a response that there was succesful retrieval of all students in my GET method
	</li>
	<li>
		201 ( Created ) : This indicates that the request was succesfull, and a new resources was created. It is used in my Post method to confirm a new Student was created.
	</li>
	<li>
		202 ( Accepted ) : This means a response was accepted but not necessarily completed. It is used to confirm a student has been deleted in my project.
	</li>
	<li>
		400 ( Bad Request ) : This means that a request was malformed. Either bad syntax or invalid parameters. In my project, it is usually that an ID was invalid.
	</li>
	<li>
		404 ( Not Found ) : This code means that the request resource that you are trying to retrieve could not be found. In my project it is used to get a student or delete a student based on an ID
	</li>
	<li>
		500 ( Internal Error ) : This is a server side error. It is used to indicate that something internally has failed when trying to retrieve a resource or perform a certain request.
	</li>
</ul>

</br>
</br>

<h5> 2.	Does the service you created in this assignment conform to all REST principals? Explain why. </h5>
<ul> 
	<li>
		No I do not believe I conformed to the Caching constraint. No where in our project do we include a caching directive to improve performance or reduce server load.
	</li>
	<li>
		Hypermedia as the Engine of Application State is also not implemented. This means that we didnt include links in our API responses to guide a client to some type of thing they can complete.
	</li>
	<li>
		It is resource based as an example the Student is accessed using unique IDs in the URL.
	</li>
	<li>
		It is stateless as the server does not store any client context between requests. 
	</li>
	<li>
		It is Uniform interface'd based onthe fact that we use standard HTTP methods to retrieve resources.
	</li>
	<li>
		It is self-descripteive... We respond with meaniningful http status code for all endpoints.
	</li>
</ul>
