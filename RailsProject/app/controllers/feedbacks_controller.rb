class FeedbacksController < ApplicationController
		before_action :no_content

	def no_content
		render :text => "", :layout => true
	end

	def new
	end
end